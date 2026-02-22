using UnityEngine;

public class AuthService : MonoBehaviour
{
    public void Login(string username, string password, bool rememberMe)
    {
        Debug.Log("Login Attempt: \n" +
                  "Username: " + username + " " + "Password: " + password + " " + "Remember: " + rememberMe);
    }
}