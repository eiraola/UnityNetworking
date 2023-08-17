using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;

public enum EAuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
}

public static class AuthenticationWrapper 
{
    public static EAuthState AuthState { get; private set; } = EAuthState.NotAuthenticated;
    public static async Task<EAuthState> DoAuth(int attempts = 5)
    {
        if (AuthState == EAuthState.Authenticated)
        {
            return AuthState;
        }
        if (AuthState == EAuthState.Authenticating)
        {
            await Authenticating();
            Debug.LogWarning("Already authenticating");
            return AuthState;
        }
        return await SignInAnonimously(attempts);
    }
    private static async Task<EAuthState> Authenticating()
    {
        while(AuthState == EAuthState.Authenticating || AuthState == EAuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }
        return AuthState;
    }
    private static async Task<EAuthState> SignInAnonimously(int attempts)
    {
        AuthState = EAuthState.Authenticating;
        int tries = 0;
        while (AuthState == EAuthState.Authenticating && tries < attempts)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = EAuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError(ex);
                AuthState = EAuthState.Error;
                
            }
            catch (RequestFailedException exc)
            {
                Debug.LogError(exc);
                AuthState = EAuthState.Error;
            }

            tries++;
            await Task.Delay(1000);
        }
        if (AuthState != EAuthState.Authenticated)
        {
            Debug.LogWarning($"Player was not signed in succesfully after {tries} tries");
            AuthState = EAuthState.TimeOut;
        }
        return AuthState;
    }
}
