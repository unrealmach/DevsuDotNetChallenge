namespace Identity.Domain.Entities;

public class Client : Person
{
    protected Client() { }                                                     

    public string ClientId { get; private set; }
    public string Password { get; private set; }
    public string Status { get; private set; }
    
    public static new Client Create() => new();                                    
                                                                                 
    public Client WithClientId(string clientId) { ClientId = clientId; return  
        this; }                                                                        
    public Client WithPassword(string password) { Password = password; return  
        this; }                                                                        
    public Client WithStatus(string status) { Status = status; return this; }  

}