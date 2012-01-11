using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NProject.Models.Domain;

namespace NProject.BLL
{
    public class CompanyService : BaseService
    {
        public Company GetCompany(int id)
        {
            return Database.Companies.FirstOrDefault(c => c.Id == id);
        }
    }
}
