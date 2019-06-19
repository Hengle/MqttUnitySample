class RegisterReq
{
    public string service;
    public string auth_type;
    public string device_id;
    public string player_id;
}

class RegisterResp
{
    public string auth_token;
}

class ChatAuthResp
{
    public string host;
    public int port;
    public string public_ch;
    public string private_ch;
    public string notice_ch;
}

class ChatAuth
{
    string host = "http://127.0.0.1:3100";
    string registerURL => $"{host}/v0/account/register";
    string chatAuthURL => $"{host}/v1/chat/auth";
    string sendPubURL => $"{host}/v1/chat/send/pub";

    string device_id = "chat-sample-1";
    string player_id = "chat-sample-1";

    public ChatAuthResp Register()
    {
        var register = SimpleRequest.Request<RegisterResp>(registerURL, new RegisterReq
        {
            service = "pop",
            auth_type = "guest",
            device_id = device_id,
            player_id = player_id
        });
        var authToken = register.auth_token;

        return SimpleRequest.Request<ChatAuthResp>(chatAuthURL, authToken: authToken);
    }

}