namespace Message.Integration.Aliyun.Clients
{
    public interface IMessageClient
    {
        void Send(string accessKeyId, string secret, MessageContent message);
    }
}
