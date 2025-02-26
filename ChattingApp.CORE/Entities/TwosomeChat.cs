using ChattingApp.CORE.DTO.TwosomeChatDTO;

namespace CORE.Entities
{
    public class TwosomeChat:Chat
    {
        public TwosomeChat()
        {
            this.Messages = new List<Message>();
            this.Members = new List<ChatMember>();
        }
    }
}
