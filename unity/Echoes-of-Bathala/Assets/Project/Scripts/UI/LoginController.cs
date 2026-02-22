using UnityEngine;
using UnityEngine.UIElements;

public class LoginController : MonoBehaviour
{
    
    private Button _loginButton; 
    private TextField _usernameField;
    private TextField _passwordField;
    private Toggle  _rememberMe;
    
    [SerializeField] private AuthService authService;
    
    void OnEnable()
    {
        UIDocument uiDoc = GetComponent<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;
        
        _loginButton = root.Q<Button>("LoginButton");
        _usernameField = root.Q<TextField>("UsernameTextField");
        _passwordField = root.Q<TextField>("PasswordTextField");
        _rememberMe = root.Q<Toggle>("RememberMeBtn");
        
        if (_loginButton != null)
        {
            _loginButton.clicked += OnLoginClick;
            Debug.Log("Button Found and Linked!");
        }
        
    }

    private void OnLoginClick()
    {
        string user = _usernameField.value;
        string pass = _passwordField.value;
        bool rememberMe = _rememberMe.value;
        
        authService.Login(user, pass, rememberMe);
    }
}