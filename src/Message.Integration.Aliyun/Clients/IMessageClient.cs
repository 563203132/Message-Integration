namespace Message.Integration.Aliyun.Clients
{
    public interface IMessageClient
    {
        void Send(string phoneNumber, string code);
    }
}
