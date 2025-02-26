using ChattingApp.CORE.DTO.ContactDTO;
using ChattingApp.CORE.DTO.ResultDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApp.CORE.Interface
{
    public interface IContactRepository
    {
        IntResult Add(AddContactDTO contact);
        IntResult Delete(int id);
        ShowContactDTO GetById(int id);
        ShowContactOfUserDTO GetAllContactForUserById(string id);
    }
}
