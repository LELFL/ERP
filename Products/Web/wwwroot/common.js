window.loginUrl = window.location.origin + "/login";
window.clientUrl = window.location.origin;

window.api = {
    baseUrl: window.location.origin,

    // 异步请求适配器
    requestAdaptor: async (api) => {
        try {
            if (api.url.startsWith("/api")) {
                api.url = api.url.replace("/api", window.api.baseUrl);

                // 非 token 端点需要认证
                if (!api.url.includes("/connect/token")) {
                    // 检查并刷新 token
                    if (window.userService.isTokenExpired()) {
                        await window.userService.loginOrRefreshToken();
                    }
                    api.headers = api.headers || {};
                    api.headers["Authorization"] = window.userService.tokens.accessToken;
                }
            }
            return api;
        } catch (error) {
            console.error('Request adaptor error:', error);
            throw error;
        }
    },

    // 异步响应适配器
    responseAdaptor: (api, payload, query, request, response) => {
        try {
            // 标准响应格式处理
            if (payload?.status !== undefined && payload.msg !== undefined) {
                return payload;
            }

            // 错误处理
            if (payload?.error) {
                const msg = payload.error.details || payload.error.message || "未知错误";
                return { status: -1, msg, data: payload };
            }

            // 状态码处理
            if (response.status >= 200 && response.status < 300) {
                return { status: 0, msg: "", data: payload };
            }

            // 401 处理
            if (response.status === 401) {
                window.userService.handleUnauthorized();
            }

            // 其他错误
            return {
                status: -1,
                msg: `请求失败 (${response.status})`,
                data: payload
            };
        } catch (error) {
            console.error('Response adaptor error:', error);
            return { status: -1, msg: '系统错误', data: null };
        }
    }
};

window.userService = {
    // Token 管理
    tokens: {
        get accessToken() { return "Bearer " + localStorage.getItem("access_token") },
        get refreshToken() { return localStorage.getItem("refresh_token") },
        get expiresIn() { return parseInt(localStorage.getItem("expires_in")) },

        set: function (data) {
            if (!data) return;
            localStorage.setItem("access_token", data.access_token);
            localStorage.setItem("refresh_token", data.refresh_token);
            localStorage.setItem("expires_in", Date.now() + data.expires_in * 1000);
        },

        clear: function () {
            ['access_token', 'refresh_token', 'expires_in', 'user_id', 'user_name']
                .forEach(k => localStorage.removeItem(k));
        }
    },

    // 用户状态
    user: {
        get id() { return localStorage.getItem("user_id") },
        get name() { return localStorage.getItem("user_name") },
        set: function (user) {
            if (!user) return;
            localStorage.setItem("user_id", user.id);
            localStorage.setItem("user_name", user.name);
        }
    },

    // 认证状态检查
    isTokenExpired() {
        return !this.tokens.accessToken || Date.now() > this.tokens.expiresIn;
    },

    // Token 刷新流程
    async loginOrRefreshToken() {
        try {
            if (!this.tokens.refreshToken) throw new Error('No refresh token');

            const formData = new URLSearchParams();
            formData.append('grant_type', 'refresh_token');
            formData.append('refresh_token', this.tokens.refreshToken);
            formData.append('client_id', 'employee_client');
            formData.append('client_secret', 'ds1fa2d1g23sg456dsda4g6s63.-45sj');

            const response = await fetch("https://localhost:5001/connect/token", {
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: formData
            });

            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            const data = await response.json();
            if (data.error) throw new Error(data.error_description);

            this.tokens.set(data);
            return true;
        } catch (error) {
            console.error('Token refresh failed:', error);
            this.logout();
            return false;
        }
    },

    // 统一处理 401
    handleUnauthorized() {
        window.location.href = window.loginUrl;
        //try {
        //    if (await this.loginOrRefreshToken()) {
        //        // 自动重试原始请求（需要业务逻辑配合）
        //        return location.reload();
        //    }
        //} catch (error) {
        //    this.logout();
        //    window.location.href = window.loginUrl;
        //}
    },

    // 登出
    logout() {
        this.tokens.clear();
        window.location.href = window.loginUrl;
    },

    // 登录
    async login(credentials) {
        try {
            const formData = new URLSearchParams();
            formData.append('grant_type', 'password');
            formData.append('username', credentials.username);
            formData.append('password', credentials.password);
            formData.append('client_id', 'employee_client');
            formData.append('client_secret', 'ds1fa2d1g23sg456dsda4g6s63.-45sj');

            const response = await fetch("https://localhost:5001/connect/token", {
                method: "POST",
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: formData
            });

            if (!response.ok) throw new Error(`Login failed: ${response.status}`);

            const data = await response.json();
            this.tokens.set(data);
            this.user.set({ id: 'user_id_from_api', name: 'user_name_from_api' });

            return true;
        } catch (error) {
            console.error('Login failed:', error);
            throw error;
        }
    }
};