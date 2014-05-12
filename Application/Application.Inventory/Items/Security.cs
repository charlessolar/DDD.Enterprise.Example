using Demo.Library.Security;
using Demo.Library.Security.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.Items
{
    public class Security : Descriptor
    {
        public Security()
        {
            // When receiving queries of type Queries.GetItem users need permission GetItems
            // When receiving queries of type Queries.GetItem users have username beginning with Admin
            // When any action and target all securables users have username equal to Admin
            When.Receiving().Queries().OfType<Queries.GetItem>()
                .Users().HavePermission("GetItems");

            // When recieving queries of type Queries.GetItem users need permission GetItems and users need permission Reading
            When.Receiving().Queries().OfType<Queries.GetItem>(s =>
            {
                s.Users().HavePermission("GetItems");
                s.Users().HavePermission("Reading");
            });

            When.Receiving().Queries().OfType<Queries.GetItem>()
                .Users().HavePermission("Test");


            When.Executing().Functions().InNamespace("SecureSpace")
                .Users().HavePermission("Admin");


            When.Reading().Properties().InType<Queries.GetItem>("Number")
                .Users().HavePermission("GetItem");
        }
    }
}