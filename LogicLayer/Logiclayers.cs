using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataLinkLayer;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace LogicLayer
{
    
    public class AdminBLL
    {
        AdminDAL dal = new AdminDAL();
       
        public string PasswordHash(string Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }

        public int RegisterAdmin(AdminModel admin)
        {
            if (dal.IsUserExists(admin.Username))
            {
                return 2;
            }
            else
            {
               int i =dal.InsertAdmin(admin);
                if(i == 1)
                {
                    return 1;
                }
            }
            return 0;
        }
    }

    public class UserBLL : AdminBLL
    {
        UserDAL dal = new UserDAL();

        public int RegisterUser(UserModel user)
        {
            if (dal.IsUserExists(user.Username))
            {
                return 2;
            }
            else
            {
                int i = dal.InsertUser(user);
                if (i == 1)
                {
                    return 1;
                }
            }
            return 0;
        }
    }

    public class LoginBLL : AdminBLL
    {
        LoginDAL dal = new LoginDAL();

        public (int id, string loginType) LoginUser(LoginModel login)
        {
            if (dal.GetCountID(login) != 1)
                return (0, "Null");

            using (SqlDataReader dr = dal.GetUserDetails(login))
            {
                if (dr.Read())  
                {
                    string storedHash = dr["Password"].ToString();
                    bool isValid = BCrypt.Net.BCrypt.Verify(login.Password,storedHash);

                    if (isValid)
                    {
                        return (Convert.ToInt32(dr["Reg_ID"]),dr["Login_Type"].ToString());
                    }
                }
            }

            return (0, "Null");
        }

    }

    public class CategoryBLL
    {
        CategoryDAL dal = new CategoryDAL();

        public int AddCategory(CategoryModel category)
        {

            if (!dal.DoesCategoryExists(category.Name))
            {
                return dal.AddCategory(category);
            }

            return 0;
        }

        public DataTable GetCategoryDetails()
        {
            return dal.GetCategoryDetails();
        }

        public int UpdateCategory(CategoryModel category)
        {
            return dal.UpdateCategory(category);
        }

        public int DeleteCategory(int CategoryId)
        {
            return dal.DeleteCategory(CategoryId);
        }


    }

    public class ProductBLL:CategoryBLL
    {
        ProductDAL dal = new ProductDAL();
        public int AddProduct(ProductModel product)
        {
            if (!dal.DoesCategoryExists(product.Name))
            {
                return dal.AddProduct(product);
            }

            return 0;
           
        }
        public DataTable GetProductDetails(string catid=null)
        {
            return dal.GetProductDetails(catid);
        }

        public DataTable GetOneProductDetails(string pid)
        {
            return dal.GetOneProductDetails(pid);
        }

        public int UpdateProduct(ProductModel changeproduct)
        {
            return dal.UpdateProduct(changeproduct);
        }

        public int DeleteProduct(int productid)
        {
            return dal.DeleteProduct(productid);
        }

        public bool ReduceStockByOrder(int orderId)
        {
           return dal.ReduceStockByOrder(orderId);
        }
    }



    public class CartBLL
    {
        CartDAL dal = new CartDAL();

        public int AddToCart(CartModel cart,ItemModel item)
        {
            if (!dal.DoesCartExists(cart))
            {
                dal.CreateCart(cart);
                int cartid = dal.GetCartID(cart);
                return dal.AddCartItem(cartid, item);

            }
            else
            {
                int cartid = dal.GetCartID(cart);
                if (!dal.DoesProductExists(cartid, item))
                {

                    return dal.AddCartItem(cartid, item);
                }
                else
                {
                    return dal.UpdateQuantity(cartid, item);
                }
            }
        }
       
    }


    public class ViewCartBLL
    {
        ViewCartDAL dal = new ViewCartDAL();
        public int GetActiveCartId(int consumerid)
        {
            return dal.GetCartID(consumerid);
        }

        public DataTable GetCartItems(int cartid)
        {
            return dal.GetCartItems(cartid);
        }

        public int GetCartTotal(int cartid)
        {
            return dal.GetCartTotal(cartid);
        }

        public void ChangeQuantity(int cartId, int productId, int changeBy)
        {
            dal.ChangeQuantity(cartId, productId, changeBy);
        }

        public int RemoveCartItem(int cartid, int productid)
        {
            return dal.RemoveItems(cartid, productid);
        }

        public void ChangeCartStatusAndRemoveItems(int consumerid)
        {
            dal.ChangeCartStatusAndRemoveItems(consumerid);
        }

    }

    public class CheckoutBLL
    {
        CheckoutDAL dal = new CheckoutDAL();

        public int Checkout(int consumerId)
        {
            return dal.Checkout(consumerId);
        }
    }

    public class BillBLL
    {
        BillDAL dal = new BillDAL();

        public DataTable GetBill(int orderId)
        {
            return dal.GetBill(orderId);
        }

        public DataTable GetBillItems(int orderId)
        {
            return dal.GetBillItems(orderId);
        }

        public bool MarkBillAsPaid(int billId)
        {
            return dal.MarkBillAsPaid(billId);       
        }
        public bool MarkBillAsPaymentFailed(int billId)
        {
            return dal.MarkBillAsPaymentFailed(billId);
        }
    }

    public class OrdersBLL
    {
        OrdersDAL dal = new OrdersDAL();

        public DataTable GetMyOrders(int consumerId)
        {
            if (consumerId <= 0)
                return null;

            return dal.GetOrdersByConsumer(consumerId);
        }

        public bool MarkOrderAsOrdered(int orderId)
        {

            return dal.MarkOrderAsOrdered(orderId);
           
        }
        public bool MarkOrderAsCancelled(int orderId)
        {

            return dal.MarkOrderAsCancelled(orderId);

        }

    }

    public class BankAccountBLL
    {
        BankAccountDAL dal = new BankAccountDAL();

        public DataTable GetUserAccounts(int consumerId)
        {
            return dal.GetAccountsByUser(consumerId);
        }

        public int DeleteAccount(int accountId, int consumerId)
        {
            return dal.DeleteAccount(accountId, consumerId);
        }

        public int AddAccount(int consumerId, string accountNo)
        {
            if (dal.AccountExists(accountNo))
                return -1; // duplicate account

            return dal.AddAccount(consumerId, accountNo);
        }
    }

    public class FeedbackBLL
    {
        FeedbackDAL dal = new FeedbackDAL();

        public bool SubmitFeedback(int consumerId, string feedbackMessage)
        {
            int rows = dal.InsertFeedback(consumerId, feedbackMessage);
            return rows > 0;
        }

        public DataTable GetAllFeedback()
        {
            return dal.GetAllFeedback();
        }

        public bool SendReply(int feedbackId, string response)
        {
            int rows = dal.ReplyToFeedback(feedbackId, response);
            if (rows <= 0)
                return false;

            string email = dal.GetConsumerEmailByFeedbackId(feedbackId);
            if (email != "null")
            {
                SendEmail(email, response);
            }

            return true;
        }
        private void SendEmail(string toEmail, string replyMessage)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("joelvargistthomas@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = "Reply to your feedback";
            mail.Body =
                "Dear User,\n\n" +
                "Thank you for your feedback.\n\n" +
                "Admin Reply:\n" +
                replyMessage +
                "\n\nRegards,\nAdmin";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(
                "joelvargistthomas@gmail.com",
                "vkefdeaodkmkylcq"
            );
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }

    public class ConsumerBLL
    {
        ConsumerDAL dal = new ConsumerDAL();

        public DataTable GetAllConsumers()
        {
            return dal.GetAllConsumers();
        }

        public bool ToggleStatus(int consumerId, bool currentStatus)
        {
            bool newStatus = !currentStatus;
            int rows = dal.UpdateConsumerStatus(consumerId, newStatus);
            return rows > 0;
        }
    }
}
