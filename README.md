# ERP 项目 README

## 项目概述
本项目是一个基于 .NET Aspire 构建的云原生微服务应用，采用了 Clean Architecture 和 CQRS 模式，以实现模块化、可维护和可扩展的系统架构。身份验证部分使用 OpenIddict 实现，目前已完成身份验证服务（包含 RBAC 授权），商品管理服务仅实现了基本的增删改查功能，并集成了身份验证服务器进行身份验证，但尚未完全完成。

## 技术栈
### 核心框架
- **.NET 9.0**：项目的基础开发框架。
- **.NET Aspire**：用于构建云原生应用，简化微服务的部署和管理。

### 身份验证与授权
- **OpenIddict**：实现 OAuth 2.0 和 OpenID Connect 协议，提供身份验证和授权功能。
- **RBAC（基于角色的访问控制）**：在身份验证服务中实现，用于管理用户权限。

### 数据库与缓存
- **MySQL**：作为主要的关系型数据库，存储用户、商品等数据。
- **Redis**：用于缓存用户权限信息，提高系统性能。

### 其他依赖
- **MediatR**：实现 CQRS 模式中的命令和查询处理。
- **Ardalis.GuardClauses**：用于参数验证，保护代码的健壮性。
- **EPPlus**：用于处理 Excel 文件。

## 项目结构
```plaintext
.git/
.gitattributes
.github/
.gitignore
AppHost/
  ELF.AppHost/          # 应用主机项目，负责启动和管理微服务
  ELF.ServiceDefaults/  # 共享服务配置项目
ERP.sln
Identity/
  .filenesting.json
  Application/          # 身份验证服务的应用层
  Domain/               # 身份验证服务的领域层
  Identity.sln
  Infrastructure/       # 身份验证服务的基础设施层
  Web/                  # 身份验证服务的 Web 层
LICENSE.txt
Products/
  .filenesting.json
  Application/          # 商品管理服务的应用层
  Domain/               # 商品管理服务的领域层
  ELF/
  Identity.sln
  Infrastructure/       # 商品管理服务的基础设施层
  Web/                  # 商品管理服务的 Web 层
README.md
```

## 主要功能模块
### 身份验证服务
- **用户登录**：支持密码授权和刷新令牌授权。
- **RBAC 授权**：通过 Redis 缓存用户权限信息，提高授权效率。
- **权限管理**：根据用户角色分配不同的权限。

### 商品管理服务
- **基本 CRUD 操作**：实现商品的新增、删除、修改和查询功能。
- **身份验证集成**：调用身份验证服务器进行身份验证，确保只有授权用户可以访问。

## 部署与运行
### 环境要求
- **.NET 9.0 SDK**
- **Docker**：用于运行 MySQL 和 Redis 容器。

### 步骤
1. 克隆项目到本地：
```bash
git clone https://github.com/your-repo/ERP.git
cd ERP
```

2. 启动应用主机：
```bash
dotnet run --project AppHost/ELF.AppHost/ELF.AppHost.csproj
```

3. 访问身份验证服务：
```plaintext
https://localhost:8001
```

4. 访问商品管理服务：
```plaintext
https://localhost:8002
```

## 注意事项
- 商品管理服务尚未完全完成，部分功能可能存在缺失或不稳定的情况。
- 请确保 Docker 服务已启动，以便正确运行 MySQL 和 Redis 容器。

## 贡献指南
如果你想为项目做出贡献，请遵循以下步骤：
1. Fork 本仓库。
2. 创建一个新的分支：`git checkout -b feature/your-feature`。
3. 提交你的更改：`git commit -m 'Add some feature'`。
4. 推送至分支：`git push origin feature/your-feature`。
5. 提交 Pull Request。

## 免责声明
此 README 由 AI 生成，可能存在错漏，仅供参考。如果你在使用过程中遇到问题，请查阅项目代码或提交 Issue 反馈。