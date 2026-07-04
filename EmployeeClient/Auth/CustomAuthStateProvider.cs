using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace EmployeeClient.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentUser.Identity!.IsAuthenticated)
            {
                return new AuthenticationState(_currentUser);
            }
            var token =await _js.InvokeAsync<string>("localStorage.getItem", "token");
            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);
            return new AuthenticationState(new ClaimsPrincipal(identity));

        }


        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims=new List<Claim>();
            var payload = jwt.Split(".")[1];
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }
            var jsonBytes = Convert.FromBase64String(payload);
            var keyValuePairs =
                JsonSerializer
                .Deserialize<Dictionary<string,JsonElement>>(jsonBytes);
            if (keyValuePairs == null) return claims;


            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Value.ValueKind==JsonValueKind.Array)
                {
                    foreach (var element in kvp.Value.EnumerateArray())
                        claims.Add(new Claim(kvp.Key, element.ToString()));
                }
                else
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
     
            }
            return claims;
        }
        public void NotifyUserLoggedIn(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user=new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLoggedOut()
        {
            var anonymous =
                new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
