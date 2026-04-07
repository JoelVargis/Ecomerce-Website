using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AdminModel
    {
        public string Name { get; set; }
        public string Phone_No { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }       

    }
    public class UserModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone_No { get; set; }
        public string Email { get; set; }
        public string Pincode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CategoryModel
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Id { get; set; }
    }

    public class ProductModel
    {
        public string Name { get; set; }
        public string CategoryId { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string ImageName { get; set; }
        public string Stock { get; set; }
        public string Status { get; set; }
     
    }

    public class CartModel
    {
        public string ConsumerId { get; set; }
        public string CartStatus { get; set; }
    }

    public class ItemModel
    {
        public string Cartid { get; set; }
        public int Id { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }      
    }
}
