using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;
using System.Data;



namespace DataLinkLayer
{
    public class AdminDAL
    {
        DBhelper db = new DBhelper();

        public bool IsUserExists(String Username)
        {
            string q = "SELECT COUNT(*) FROM Login WHERE Username=@Username";
            SqlParameter[] p =
            {
            new SqlParameter("@Username",Username)
            };
            return Convert.ToInt32(db.fun_executescalar(q, p)) > 0;
        }

        public int InsertAdmin(AdminModel admin)
        {
            string que = "SELECT MAX(Reg_ID) FROM Login";
            string str = db.fun_executescalar(que);
            int regid = 0;
            if (str =="")
            {
                regid = 1;
            }
            else
            {
                int newregid = Convert.ToInt32(str);
                regid = newregid + 1;
            }
             que = @"INSERT INTO Admin(Admin_ID,Admin_Name,Admin_Phone_No,Admin_Email) VALUES(@Admin_ID,@Admin_Name,@Admin_Phone_No,@Admin_Email)";

            SqlParameter[] p1 =
            {
                new SqlParameter("@Admin_ID",regid),
                new SqlParameter("@Admin_Name", admin.Name),
                new SqlParameter("@Admin_Phone_No", admin.Phone_No),
                new SqlParameter("@Admin_Email", admin.Email)
            };
            int i = db.fun_executenonquery(que, p1);
            if (i == 1)
            {
                que = "INSERT INTO Login VALUES(@Reg_ID,@Username,@Password,@LoginType)";

                SqlParameter[] p2 =
                {
                      new SqlParameter("@Reg_ID", regid),
                      new SqlParameter("@Username",admin.Username),
                      new SqlParameter("@Password",admin.Password),
                      new SqlParameter("@LoginType","Admin")                
                };

                int j = db.fun_executenonquery(que, p2);
                if (i == 1 && j == 1)
                {
                    return 1;
                }
                
            }
            return 0;
        }
    }

    public class UserDAL : AdminDAL
    {
        DBhelper db = new DBhelper();
        public int InsertUser(UserModel user)
        {
            string que = "SELECT MAX(Reg_ID) FROM Login";
            string str = db.fun_executescalar(que);
            int regid = 0;
            if (str == "")
            {
                regid = 1;
            }
            else
            {
                int newregid = Convert.ToInt32(str);
                regid = newregid + 1;
            }
            que = @"INSERT INTO Consumer VALUES (@Consumer_ID, @Consumer_Name, @Consumer_Address, @Consumer_Phone_No,@Consumer_Email, @Consumer_Pincode, @Consumer_Status)";


            SqlParameter[] p3 =
              {
                new SqlParameter("@Consumer_ID", regid),
                new SqlParameter("@Consumer_Name", user.Name),
                new SqlParameter("@Consumer_Address", user.Address),
                new SqlParameter("@Consumer_Phone_No", user.Phone_No),
                new SqlParameter("@Consumer_Email", user.Email),
                new SqlParameter("@Consumer_Pincode", user.Pincode),
                new SqlParameter("@Consumer_Status",1)
            };

            int i = db.fun_executenonquery(que, p3);
            if (i == 1)
            {
                que = "INSERT INTO Login VALUES(@Reg_ID,@Username,@Password,@LoginType)";

                SqlParameter[] p4 =
                {
                      new SqlParameter("@Reg_ID", regid),
                      new SqlParameter("@Username",user.Username),
                      new SqlParameter("@Password",user.Password),
                      new SqlParameter("@LoginType","User")
                };

                int j = db.fun_executenonquery(que, p4);
                if (i == 1 && j == 1)
                {
                    return 1;
                }

            }
            return 0;
        }

    }

    public class LoginDAL 
    {
        DBhelper db = new DBhelper();
        string q = "";
        public int GetCountID(LoginModel login)
        {
            string q = "SELECT COUNT(*) FROM Login WHERE Username=@Username";

            SqlParameter[] p5 =
            {
                new SqlParameter("@Username",login.Username),
            };
            int i = Convert.ToInt32(db.fun_executescalar(q, p5));
            Console.WriteLine(i);
            return i;

        }

        public SqlDataReader GetUserDetails(LoginModel login)
        {
            q = "SELECT Reg_ID,Login_Type,Password FROM Login WHERE Username=@Username";
            SqlParameter[] p6 =
            {
                new SqlParameter("@Username",login.Username),
            };
            return db.fun_executereader(q, p6);

        }

    }

    public class CategoryDAL
    {
        DBhelper db = new DBhelper();

        public bool DoesCategoryExists(String CategoryName)
        {
            string q = "SELECT COUNT(*) FROM Category WHERE Category_Name=@Category_Name";
            SqlParameter[] p =
            {
            new SqlParameter("@Category_Name",CategoryName)
            };
            return Convert.ToInt32(db.fun_executescalar(q, p)) > 0;
        }

        public int AddCategory(CategoryModel category)
        {
            string q = "INSERT INTO Category VALUES(@Category_Name,@Category_Description,@Category_Image,@Category_Status)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Category_Name",category.Name),
                new SqlParameter("@Category_Description",category.Description),
                new SqlParameter("@Category_Image",category.ImageName),
                new SqlParameter("@Category_Status","Available")
            };

            return db.fun_executenonquery(q, parameters);
        }

        public DataTable GetCategoryDetails()
        {
            string q = "SELECT * FROM Category";

            return db.fun_dataadaptertable(q);
        }


        public int UpdateCategory(CategoryModel category)
        {
            string query = @"UPDATE Category
                     SET Category_Name = @Name,
                         Category_Description = @Description,
                         Category_Image = @Image,
                         Category_Status = @Status
                     WHERE Category_ID = @CategoryId";

            SqlParameter[] p =
            {
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@Description", category.Description),
                new SqlParameter("@Image", category.ImageName),
                new SqlParameter("@Status", category.Status),
                new SqlParameter("@CategoryId", category.Id)
            };

            return db.fun_executenonquery(query, p);
        }


        public int DeleteCategory(int categoryId)
        {
            string query = "DELETE FROM Category WHERE Category_ID = @CategoryId";

            SqlParameter[] p =
            {
                new SqlParameter("@CategoryId", categoryId)
            };

            return db.fun_executenonquery(query, p);
        }




    }


    public class ProductDAL : CategoryDAL
    {
        DBhelper db = new DBhelper();

        public bool DoesProductExists(String ProductName)
        {
            string q = "SELECT COUNT(*) FROM Product WHERE Product_Name=@Product_Name";
            SqlParameter[] p =
            {
            new SqlParameter("@Product_Name",ProductName)
            };
            return Convert.ToInt32(db.fun_executescalar(q, p)) > 0;
        }
        public int AddProduct(ProductModel product)
        {
            String q = "INSERT INTO product VALUES(@Category_ID,@Product_Name,@Product_Price,@Product_Description,@Product_Stock,@Product_Image,@Product_Status)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Category_ID",product.CategoryId),
                new SqlParameter("@Product_Name",product.Name),
                new SqlParameter("@Product_Price",product.Price),
                new SqlParameter("@Product_Description",product.Description),
                new SqlParameter("@Product_Stock",product.Stock),
                new SqlParameter("@Product_Image",product.ImageName),
                new SqlParameter("@Product_Status",product.Status)

            };

            return db.fun_executenonquery(q, parameters);

        }

        public DataTable GetProductDetails(string catid)
        {
            string q = "";
            if (catid != null)
            {
                q = "SELECT * FROM Product WHERE Category_ID=@Category_ID";

                SqlParameter[] p1 =
                {
                new SqlParameter("@Category_ID",catid)
                };
                return db.fun_dataadaptertable(q, p1);
            }
            q = "SELECT * FROM Product";


            return db.fun_dataadaptertable(q);

        }

        public DataTable GetOneProductDetails(string pid)
        {
            string q = "SELECT * FROM Product WHERE Product_ID=@Product_ID";

            SqlParameter[] p1 =
               {
                new SqlParameter("@Product_ID",pid)
                };
            return db.fun_dataadaptertable(q, p1);

        }

        public int UpdateProduct(ProductModel changeproduct)
        {
            string q = "UPDATE product SET Product_Name=@Product_Name,Product_Description=@Product_Description,Product_Price=@Product_Price,Product_Image=@Product_Image,Product_Stock=@Product_Stock,Product_Status=@Product_status WHERE Product_ID=@Product_ID";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Product_Name",changeproduct.Name),
                new SqlParameter("@Product_Description",changeproduct.Description),
                new SqlParameter("@Product_Price",changeproduct.Price),
                new SqlParameter("@Product_Image",changeproduct.ImageName),
                new SqlParameter("@Product_Stock",changeproduct.Stock),
                new SqlParameter("@Product_Status",changeproduct.Status),
                new SqlParameter("@Product_ID",changeproduct.Id),
            };
            return db.fun_executenonquery(q, parameters);
        }


        public int DeleteProduct(int productid)
        {
            string q = "DELETE FROM product WHERE Product_ID=@pid";

            SqlParameter[] parameters =
            {
                new SqlParameter("@pid",productid),
            };

            return db.fun_executenonquery(q, parameters);
        }
        public bool ReduceStockByOrder(int orderId)
        {
            string query =
                @"UPDATE P
                  SET P.Product_Stock = P.Product_Stock - OI.Oitems_Quantity
                  FROM Product P
                  INNER JOIN OrderItems OI 
                      ON P.Product_ID = OI.Product_ID
                  WHERE OI.Order_ID = @OrderId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderId", orderId)
            };

            int rows = db.fun_executenonquery(query, parameters);
            return rows > 0;


        }

    }
    public class CartDAL
    {
        DBhelper db = new DBhelper();
        public int CreateCart(CartModel cart)
        {
            string q = "INSERT INTO Cart VALUES(@Consumer_ID,@status)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Consumer_ID",cart.ConsumerId),
                new SqlParameter("@status","Active")

            };

            return db.fun_executenonquery(q, parameters);
        }

        public bool DoesCartExists(CartModel cart)
        {
            string q = "SELECT Count(*) FROM Cart WHERE Consumer_ID=@Cid AND Cart_Status=@status";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Cid",cart.ConsumerId),
                new SqlParameter("@status","Active")

            };

            int result = Convert.ToInt32(db.fun_executescalar(q, parameters));

            if (result <= 0 )
            {
                return false;
            }

            return true;
        }

        public int GetCartID(CartModel cart)
        {
            string q = "SELECT Cart_ID FROM Cart WHERE Consumer_ID=@Cid AND Cart_Status=@status";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Cid",cart.ConsumerId),
                 new SqlParameter("@status","Active")

            };

            return Convert.ToInt32(db.fun_executescalar(q, parameters));

           
        }

       
        public int AddCartItem(int cartid,ItemModel item)
        {
            string q = "INSERT INTO CartItems (Cart_ID, Product_ID, Item_Quantity, Item_Price) VALUES(@Cart_ID,@Product_ID,@Item_Quantity,@Item_Price)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Cart_ID",cartid),
                new SqlParameter("@Product_ID",item.Id),
                new SqlParameter("@Item_Quantity",item.Quantity),
                new SqlParameter("@Item_Price",item.Price),
            };
            return db.fun_executenonquery(q, parameters);
        }
        
        public bool DoesProductExists(int cartid,ItemModel item)
        {
            string q = "SELECT COUNT(*) FROM CartItems WHERE Cart_ID = @cartId AND Product_ID = @pid";

            SqlParameter[] parameters =
            {
                new SqlParameter("@pid",item.Id),
                new SqlParameter("@cartId",cartid)
            };

            int result = Convert.ToInt32(db.fun_executescalar(q, parameters));

            if (result == 0)
            {
                return false;
            }
            return true;

        }

        public int UpdateQuantity(int cartid,ItemModel item)
        {
            string q = "UPDATE CartItems SET Item_Quantity = Item_Quantity + @qty WHERE Cart_ID = @cartId AND Product_ID = @pid";

            SqlParameter[] parameters =
            {
                new SqlParameter("@qty",item.Quantity),
                new SqlParameter("@pid",item.Id),
                new SqlParameter("@cartId",cartid)
             
            };
            return db.fun_executenonquery(q, parameters);

        }
    }




    public class ViewCartDAL 
    {
        DBhelper db = new DBhelper();

        public int GetCartID(int consumerid)
        {
            string q = "SELECT Cart_ID FROM Cart WHERE Consumer_ID=@Cid AND Cart_Status=@status";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Cid",consumerid),
                 new SqlParameter("@status","Active")

            };

            string result = db.fun_executescalar(q, parameters);

            if (result == "null")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(result);
            }           

        }
        public DataTable GetCartItems(int cartid)
        {
            string q = "SELECT pr.Product_Image,pr.Product_Name,ci.Product_ID,ci.Item_Price,ci.Item_Quantity,ci.Item_Subtotal FROM CartItems ci JOIN Product pr ON ci.Product_ID = pr.Product_ID WHERE ci.Cart_ID=@cid";

            SqlParameter[] parameters =
            {
               
                new SqlParameter("@cid",cartid)

            };

            return db.fun_dataadaptertable(q, parameters);
        }


        public int GetCartTotal(int cartid)
        {
            string q = "SELECT ISNULL(SUM(Item_Subtotal), 0) FROM CartItems WHERE Cart_ID=@cid";

            SqlParameter[] parameters =
            {

                new SqlParameter("@cid",cartid)

            };

            return Convert.ToInt32(db.fun_executescalar(q, parameters));
        }

        public int RemoveItems(int cartid,int productid)
        {
            string q = "DELETE FROM CartItems WHERE Cart_ID=@cid AND Product_ID=@pid";

            SqlParameter[] parameters =
            {

                new SqlParameter("@cid",cartid),
                new SqlParameter("@pid",productid)

            };

            return db.fun_executenonquery(q, parameters);
        }

        public void ChangeQuantity(int cartId, int productId, int changeBy)
        {
            string q = "UPDATE CartItems SET Item_Quantity = Item_Quantity + @qty WHERE Cart_ID = @cartId AND Product_ID = @pid";
            SqlParameter[] parameters =
            {
                new SqlParameter("@qty",changeBy),
                new SqlParameter("@pid",productId),
                new SqlParameter("@cartId",cartId)

            };
            db.fun_executenonquery(q, parameters);

            string q1 = "DELETE FROM CartItems WHERE Cart_ID = @cartId AND Product_ID = @pid AND Item_Quantity <= 0";

            SqlParameter[] parameters1 =
            {
                new SqlParameter("@pid",productId),
                new SqlParameter("@cartId",cartId)
            };
            db.fun_executenonquery(q1, parameters1);

        }

        public void ChangeCartStatusAndRemoveItems(int consumerid)
        {
            string q = "SELECT TOP 1 Cart_ID FROM Cart WHERE Consumer_ID = @uid AND Cart_Status = 'Active' ORDER BY Cart_ID DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@uid",consumerid)

            };

            int cartId = Convert.ToInt32(db.fun_executescalar(q, parameters));

            q = "UPDATE Cart SET Cart_Status = 'Ordered' WHERE Cart_ID = @cid";

            SqlParameter[] parameters7 =
            {
                new SqlParameter("@cid",cartId)

            };

            db.fun_executenonquery(q, parameters7);







            q = "DELETE FROM CartItems WHERE Cart_ID=@cid";

            SqlParameter[] parameters8 =
            {
                new SqlParameter("@cid",cartId)

            };

            db.fun_executenonquery(q, parameters8);
        }


    }



    public class CheckoutDAL
    {

        DBhelper db = new DBhelper();
        public int Checkout(int consumerid)
        {
            string q = "SELECT TOP 1 Cart_ID FROM Cart WHERE Consumer_ID = @uid AND Cart_Status = 'Active' ORDER BY Cart_ID DESC";

            SqlParameter[] parameters =
            {
                new SqlParameter("@uid",consumerid)
               
            };

            int cartId = Convert.ToInt32(db.fun_executescalar(q, parameters));









            q = "SELECT COUNT(*) FROM CartItems WHERE Cart_ID = @cid";

            SqlParameter[] parameters1 =
            {
                new SqlParameter("@cid",cartId)

            };

            int itemCount = Convert.ToInt32(db.fun_executescalar(q, parameters1));

            if (itemCount <= 0)
            {
                return 0;
            }





            q = "INSERT INTO Orders (Consumer_ID, Order_Status) VALUES(@uid,'Payment Pending') SELECT SCOPE_IDENTITY()";

            SqlParameter[] parameters2 =
            {
                new SqlParameter("@uid",consumerid)

            };
           
            int orderId= Convert.ToInt32(db.fun_executescalar(q, parameters2));






            q = "INSERT INTO OrderItems (Order_ID, Product_ID, Oitems_Quantity, Oitem_Price, Oitems_Subtotal) SELECT @oid, Product_ID, Item_Quantity, Item_Price, Item_Subtotal FROM CartItems WHERE Cart_ID = @cid";

            SqlParameter[] parameters3 =
            {
                new SqlParameter("@cid",cartId),
                new SqlParameter("@oid",orderId)
            };

            db.fun_executenonquery(q, parameters3);







            q = "SELECT ISNULL(SUM(Item_Subtotal),0) FROM CartItems WHERE Cart_ID = @cid";

            SqlParameter[] parameters6 =
            {
                new SqlParameter("@cid",cartId)

            };

            int grandTotal = Convert.ToInt32(db.fun_executescalar(q, parameters6));






            q = "INSERT INTO Bill (Order_ID, Bill_GrandTotal, Bill_Status) VALUES(@oid, @total,'Payment Pending')";

            SqlParameter[] parameters5 =
            {
                new SqlParameter("@total",grandTotal),
                new SqlParameter("@oid",orderId)
            };

            db.fun_executenonquery(q, parameters5);





            //q = "UPDATE Cart SET Cart_Status = 'Ordered' WHERE Cart_ID = @cid";

            //SqlParameter[] parameters7 =
            //{
            //    new SqlParameter("@cid",cartId)

            //};

            //db.fun_executenonquery(q, parameters7);







            //q = "DELETE FROM CartItems WHERE Cart_ID=@cid";

            //SqlParameter[] parameters8 =
            //{
            //    new SqlParameter("@cid",cartId)

            //};

            //db.fun_executenonquery(q, parameters8);

            return orderId;
        }
    }



    public class BillDAL
    {

        DBhelper db = new DBhelper();
        public DataTable GetBill(int orderId)
        {
            string q = @"SELECT Bill_ID, Bill_Date, Bill_Status, Bill_GrandTotal
                         FROM Bill WHERE Order_ID=@oid";

            SqlParameter[] parameters =
            {
                new SqlParameter("@oid",orderId)

            };

            return db.fun_dataadaptertable(q, parameters);
        }

        public DataTable GetBillItems(int orderId)
        {
            string q = @"SELECT p.Product_Name,
                                oi.Oitems_Quantity,
                                oi.Oitem_Price,
                                oi.Oitems_Subtotal
                         FROM OrderItems oi
                         JOIN Product p ON p.Product_ID = oi.Product_ID
                         WHERE oi.Order_ID=@oid";

            SqlParameter[] parameters1 =
            {
                new SqlParameter("@oid",orderId)

            };

            return db.fun_dataadaptertable(q, parameters1);

        }

        public bool MarkBillAsPaid(int billId)
        {
            string query = @"
                UPDATE Bill
                SET Bill_Status = 'Paid'
                WHERE Bill_ID = @BillId
                  AND Bill_Status <> 'Paid'";

            SqlParameter[] parameters =
            {
                new SqlParameter("@BillId", billId)
            };

            int rows = db.fun_executenonquery(query, parameters);

            return rows > 0;
        }
        public bool MarkBillAsPaymentFailed(int billId)
        {
            string query = @"
                UPDATE Bill
                SET Bill_Status = 'Payment Failed'
                WHERE Bill_ID = @BillId
                  AND Bill_Status <> 'Paid'";

            SqlParameter[] parameters =
            {
                new SqlParameter("@BillId", billId)
            };

            int rows = db.fun_executenonquery(query, parameters);

            return rows > 0;
        }
    }


    public class OrdersDAL
    {
        DBhelper db = new DBhelper();

        /* =========================
           GET ALL ORDERS BY USER
        ========================== */
        public DataTable GetOrdersByConsumer(int consumerId)
        {
            string query = @"SELECT O.Order_ID,
                                    O.Order_Date,
                                    O.Order_Status,
                                    SUM(OI.Oitems_Subtotal) AS Order_Total
                                FROM Orders O
                                INNER JOIN OrderItems OI ON O.Order_ID = OI.Order_ID
                                WHERE O.Consumer_ID = @ConsumerID
                                GROUP BY O.Order_ID, O.Order_Date, O.Order_Status
                                ORDER BY O.Order_Date DESC";

            SqlParameter[] parameters =
            {
        new SqlParameter("@ConsumerID", consumerId)
            };

            return db.fun_dataadaptertable(query, parameters);
        }

        public bool MarkOrderAsOrdered(int orderId)
        {
            string query = "UPDATE Orders SET Order_Status = 'Order Placed' WHERE Order_ID = @OrderId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderId", orderId)
            };

            int rows = db.fun_executenonquery(query, parameters);

            return rows > 0;
        }
        public bool MarkOrderAsCancelled(int orderId)
        {
            string query = "UPDATE Orders SET Order_Status = 'Payment Failed' WHERE Order_ID = @OrderId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@OrderId", orderId)
            };

            int rows = db.fun_executenonquery(query, parameters);

            return rows > 0;
        }


    }

    //banking
    public class BankAccountDAL
    {
        DBhelper db = new DBhelper();

        public DataTable GetAccountsByUser(int consumerId)
        {
            string q = @"SELECT Account_ID, Account_Number
                         FROM Bank_Account
                         WHERE Consumer_ID = @uid";

            SqlParameter[] parameters =
            {
                new SqlParameter("@uid", consumerId)
            };

            return db.fun_dataadaptertable(q, parameters);
        }

        public int DeleteAccount(int accountId, int consumerId)
        {
            string q = @"DELETE FROM Bank_Account
                         WHERE Account_ID = @aid AND Consumer_ID = @uid";

            SqlParameter[] parameters1 =
            {
                new SqlParameter("@aid", accountId),
                new SqlParameter("@uid", consumerId)
            };

            return db.fun_executenonquery(q, parameters1);
        }

        public int AddAccount(int consumerId, string accountNo)
        {
            string q = @"INSERT INTO Bank_Account (Consumer_ID, Account_Number)
                         VALUES (@uid, @acc)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@uid", consumerId),
                new SqlParameter("@acc", accountNo)
            };

            return db.fun_executenonquery(q, parameters);
        }

        public bool AccountExists(string accountNo)
        {
            string q = "SELECT COUNT(*) FROM Bank_Account WHERE Account_Number=@acc";

            SqlParameter[] parameters =
            {
                new SqlParameter("@acc", accountNo)
            };

            int count = Convert.ToInt32(db.fun_executescalar(q, parameters));
            return count > 0;
        }



    }
    //feedback
    public class FeedbackDAL
    {
        DBhelper db = new DBhelper();

        public int InsertFeedback(int consumerId, string feedbackMessage)
        {
            string query =
                @"INSERT INTO Feedback 
                  (Consumer_ID, Feedback_Message, Feedback_Status)
                  VALUES 
                  (@Consumer_ID, @Feedback_Message, 'Pending')";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Consumer_ID", consumerId),
                new SqlParameter("@Feedback_Message", feedbackMessage)
            };

            return db.fun_executenonquery(query, parameters);
        }
        public DataTable GetAllFeedback()
        {
            string query =
                @"SELECT F.Feedback_ID,
                         C.Consumer_Name,
                         F.Feedback_Message,
                         F.Feedback_Response,
                         F.Feedback_Status
                  FROM Feedback F
                  INNER JOIN Consumer C
                      ON F.Consumer_ID = C.Consumer_ID
                  ORDER BY F.Feedback_ID DESC";

            return db.fun_dataadaptertable(query);
        }

        public int ReplyToFeedback(int feedbackId, string response)
        {
            string query =
                @"UPDATE Feedback
                  SET Feedback_Response = @Response,
                      Feedback_Status = 'Responded'
                  WHERE Feedback_ID = @FeedbackId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Response", response),
                new SqlParameter("@FeedbackId", feedbackId)
            };

            return db.fun_executenonquery(query, parameters);
        }

        public string GetConsumerEmailByFeedbackId(int feedbackId)
        {
            string query =
                @"SELECT C.Consumer_Email
          FROM Feedback F
          INNER JOIN Consumer C
              ON F.Consumer_ID = C.Consumer_ID
          WHERE F.Feedback_ID = @FeedbackId";

            SqlParameter[] parameters =
            {
        new SqlParameter("@FeedbackId", feedbackId)
            };

            return db.fun_executescalar(query, parameters);
        }

    }

    public class ConsumerDAL
    {
        DBhelper db = new DBhelper();

        public DataTable GetAllConsumers()
        {
            string query = "SELECT * FROM Consumer ORDER BY Consumer_ID DESC";
            return db.fun_dataadaptertable(query);
        }

        public int UpdateConsumerStatus(int consumerId, bool status)
        {
            string query =
                @"UPDATE Consumer
                  SET Consumer_Status = @Status
                  WHERE Consumer_ID = @ConsumerId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@ConsumerId", consumerId)
            };

            return db.fun_executenonquery(query, parameters);
        }
    }


}
