using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tic_Tac_Toe.web.model;
using Tic_Tac_Toe.web.service;

namespace Tic_Tac_Toe.web.middleware;

/// Атрибут для авторизации пользователей через Basic Authentication
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UserAuthenticatorAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Пропускаем проверку для Razor Pages (они используют другую модель авторизации)
        if (context.ActionDescriptor is Microsoft.AspNetCore.Mvc.RazorPages.CompiledPageActionDescriptor)
        {
            await Task.CompletedTask;
            return;
        }

        // Проверяем атрибут [AllowAnonymous] через endpoint metadata (самый простой и надежный способ)
        var endpoint = context.HttpContext.GetEndpoint();
        if (endpoint != null)
        {
            if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await Task.CompletedTask;
                return;
            }
        }

        // Дополнительная проверка через reflection для контроллеров
        var actionDescriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
        if (actionDescriptor != null)
        {
            // Проверяем на уровне метода (приоритет выше)
            var hasAllowAnonymousOnMethod = actionDescriptor.MethodInfo
                .GetCustomAttributes(typeof(AllowAnonymousAttribute), inherit: true)
                .Any();

            if (hasAllowAnonymousOnMethod)
            {
                await Task.CompletedTask;
                return;
            }

            // Проверяем на уровне контроллера
            var hasAllowAnonymousOnController = actionDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(AllowAnonymousAttribute), inherit: true)
                .Any();

            if (hasAllowAnonymousOnController)
            {
                await Task.CompletedTask;
                return;
            }
        }

        // Получаем AuthService из DI контейнера
        var authService = context.HttpContext.RequestServices.GetService<IAuthService>();
        
        if (authService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        // Проверяем наличие заголовка Authorization
        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            context.Result = new UnauthorizedObjectResult(
                new ErrorResponse("Authorization header is required"));
            return;
        }

        string authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrWhiteSpace(authorizationHeader))
        {
            context.Result = new UnauthorizedObjectResult(
                new ErrorResponse("Authorization header is empty"));
            return;
        }

        // Валидируем логин и пароль
        Guid? userId = authService.Authenticate(authorizationHeader);

        if (userId == null)
        {
            context.Result = new UnauthorizedObjectResult(
                new ErrorResponse("Invalid login or password"));
            return;
        }

        // Сохраняем UserId в HttpContext для использования в контроллере
        context.HttpContext.Items["UserId"] = userId.Value;

        await Task.CompletedTask;
    }
}
