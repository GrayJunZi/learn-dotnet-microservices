# Learn-NET-MicroServices

## 一、介绍

使用 .NET Core 构建微服务。

## 二、自定义认证库 AuthLibrary

(1). Permission Based

- Requirements
- Policy Provider
- Auth Handler
- Attribute

(2). Constants

- Service Name
- Feature Name
- Action Name
- Claim Name
- Permission Name
- Role Group Name
- Role Name

## 三、响应库 ResponseWrapperLibrary

(1). Custom Exceptions
- Validation
- Service UnAvailable

(2). Models
- EventBus Events
- Shared Models
- Wrappers
- Wrapper Extension

## 四、身份认证服务 IdentityService

(1). Features

- Users Management
- Roles Management
- Claims Management
- User Logins
- Token Generation
- Publish Events

(2). Key Dependencies

- AspNetCore Identity
- Response Library
- Custom Auth Library
- EFCore
- SQL Server
- MediatR
- FluentValidation
- Mapster
- MassTransit
- ...

## 五、API网关 Gateway

- Ocelot

## 六、产品服务 ProductService

(1). Features

- Product Management
- Publish Product Events

(2). Key Dependencies

- Response Library
- Custom Auth Library
- EFCore
- SQL Server
- MediatR
- FluentValidation
- Mapster
- ...

## 七、库存服务 InventoryService

