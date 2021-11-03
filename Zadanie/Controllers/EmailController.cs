using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.RefModels;

namespace Controllers
{
    public class EmailController : Controller
    {
        DBContext db = new DBContext();

        [HttpGet, ActionName("Create")]
        public ActionResult CreateOnGet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Single(c => c.ID == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;
            ViewBag.id = customer.ID;

            return View();
        }

        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOnPost([Bind(Include = "ID,EmailContent,CustomerID")] Email email, int id)
        {
            var customer = await db.Customers.SingleAsync(c => c.ID == id);
            ViewBag.id = customer.ID;
            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            Email newEmail = email;
            newEmail.Customer = customer;

            if (ModelState.IsValid)
            {
                db.Emails.Add(newEmail);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = id });
            }

            return View(email);
        }

        [HttpGet, ActionName("Edit")]
        public ActionResult EditOnGet(int? id)
        {
            Email email = db.Emails.Find(id);
            Customer customer = db.Customers.Single(c => c.ID == email.Customer.ID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (email == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerID = customer.ID;

            return View(email);
        }

        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOnPost([Bind(Include = "ID,EmailContent,CustomerID")] Email email)
        {
            int id = email.ID;

            if (email.EmailContent != null)
                return RedirectToAction("Edit", "Email", new { email = await db.Emails.FindAsync(id) });

            if (ModelState.IsValid)
            {
                db.Entry(email).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = Request.Form["CustomerID"] });
            }

            return View(email);
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteOnGet(int? id)
        {
            Email email = db.Emails.Find(id);
            Customer customer = db.Customers.Single(c => c.ID == email.Customer.ID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (email == null)
            {
                return HttpNotFound();
            }

            return View(email);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOnPost(int id)
        {
            Email email = await db.Emails.FindAsync(id);
            var customer = await db.Customers.SingleAsync(c => c.ID == email.Customer.ID);
            db.Emails.Remove(email);
            await db.SaveChangesAsync();

            return RedirectToAction("Details", "Customer", new { id = customer.ID });
        }
    }
}