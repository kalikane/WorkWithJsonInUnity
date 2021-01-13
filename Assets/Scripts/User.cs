using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class User
{
    public int id;
    public string name;
    public string username;
    public string email;
    public Adresse address;
    public string phone;
    public string website;
    public Company company;
}

