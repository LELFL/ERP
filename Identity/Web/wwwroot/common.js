window.loginUrl = window.location.origin + "/login";
window.clientUrl = window.location.origin;
window.api = {
    baseUrl: window.location.origin,
  // 全局 api 请求适配器
  // 另外在 amis 配置项中的 api 也可以配置适配器，针对某个特定接口单独处理。

  requestAdaptor(api) {
    // 支持异步，可以通过 api.mockResponse 来设置返回结果，跳过真正的请求发送
    // 此功能自定义 fetcher 的话会失效
    // api.context 中包含发送请求前的上下文信息
    if (api.url.startsWith("/api")) {
      api.url = api.url.replace("/api", window.api.baseUrl);
      // 判断token是否过期，如果过期则根据token刷新token
      if (!api.url.includes("/connect/token")) {
        if (window.userService.isTokenExpired()) {
          window.userService.loginOrRefreshToken();
        }
      }

      api.headers["Authorization"] = window.userService.getToken();
    }

    return api;
  },

  // 全局 api 适配器。
  // 另外在 amis 配置项中的 api 也可以配置适配器，针对某个特定接口单独处理。
  responseAdaptor(api, payload, query, request, response) {
    if (
      payload.status != undefined &&
      payload.msg != undefined &&
      payload.data != undefined
    ) {
      return payload;
    }
    if (payload.error) {
      var msg = payload.error.details || payload.error.message || "未知错误！";
      console.log(payload.error);
      return {
        status: -1,
        msg: msg,
        data: payload,
      };
    }

    if (response.status >= 200 && response.status < 300) {
      return {
        status: 0,
        msg: "",
        data: payload,
      };
    } else if (response.status == 401) {
      window.location.href = window.loginUrl;
      // if (window.userService.loginOrRefreshToken()) {
      //   //重新请求
      // }
    } else {
      return {
        status: -1,
        msg: response.status,
        data: payload,
      };
    }

    console.log(api, payload, query, request, response);
    return payload;
  },
};

window.userService = {
  /**
   * 设置Token、刷新token、过期事件
   */
  setToken(data) {
    console.log("setToken", data);
    localStorage.setItem("access_token", data.access_token);
    localStorage.setItem("refresh_token", data.refresh_token);
    //过期时间=当前时间+过期时间(秒)
    expireIn = new Date().getTime() + data.expires_in * 1000;
    localStorage.setItem("expires_in", expireIn);
  },
  logout() {
    localStorage.removeItem("access_token");
    localStorage.removeItem("refresh_token");
    localStorage.removeItem("expires_in");
    localStorage.removeItem("user_id");
    localStorage.removeItem("user_name");
    window.location.href = window.loginUrl;
  },
  /**
   * 获取token
   */
  getToken() {
    return "Bearer " + localStorage.getItem("access_token");
  },
  /**
   * 判断token是否过期
   */
  isTokenExpired() {
    var timespan = localStorage.getItem("expires_in");
    if (timespan > new Date().getTime()) {
      return false;
    } else {
      return true;
    }
  },
  loginOrRefreshToken() {
    //判断刷新token是否存在
    let refreshToken = localStorage.getItem("refresh_token");
    if (refreshToken && refreshToken != "undefined") {
      console.log("刷新token存在");
      //请求刷新token
      var myHeaders = new Headers();

      var urlencoded = new URLSearchParams();
      urlencoded.append("grant_type", "refresh_token");
      urlencoded.append("refresh_token", refreshToken);
      urlencoded.append("client_id", "employee_client");
      urlencoded.append("client_secret", "ds1fa2d1g23sg456dsda4g6s63.-45sj");

      var requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: urlencoded,
        redirect: "follow",
      };

      fetch("https://localhost:5001/connect/token", requestOptions)
        .then((response) => {
          return response.json();
        })
        .then((result) => {
          //刷新token成功
          if (result.error) {
            //刷新token失败
            //跳转登录
            window.location.href = window.loginUrl;
          } else {
            //更新token
            window.userService.setToken(result);
          }
        })
        .catch((error) => {
          //刷新token失败
          //跳转登录
          window.location.href = window.loginUrl;
        });

      // $.ajax({
      //   url: "/api/auth/refreshToken",
      //   type: "POST",
      //   data: {
      //     refreshToken: localStorage.getItem("refresh_token"),
      //   },
      //   success: function (data) {
      //     //刷新token成功
      //     if (data.status == 0) {
      //       //更新token
      //       localStorage.setItem("access_token", data.data.token);
      //     }
      //   },
      //   error: function (data) {
      //     //刷新token失败
      //     //跳转登录
      //     window.location.href = window.loginUrl;
      //   },
      // });
    } else {
      //跳转登录
      window.location.href = window.loginUrl;
    }
  },
  login(user) {
    localStorage.setItem("user_id", user.id);
    localStorage.setItem("user_name", user.name);
  },
  getUserInfo() {
    return {
      id: localStorage.getItem("user_id"),
      name: localStorage.getItem("user_name"),
    };
  },
};
