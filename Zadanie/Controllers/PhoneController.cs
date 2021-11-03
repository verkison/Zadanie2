using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.RefModels;

namespace Controllers
{
    public class PhoneController : Controller
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
        public async Task<ActionResult> CreateOnPost([Bind(Include = "ID,PhoneContent,CustomerID")] Phone phone, int id)
        {
            var customer = await db .Customers.SingleAsync(c => c.ID == id);
            ViewBag.id = customer.ID;
            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            Phone newPhone = phone;
            newPhone.Customer = customer;

            if (ModelState.IsValid)
            {
                db.Phones.Add(phone);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = id });
            }

            return View(phone);
        }

        [HttpGet, ActionName("Edit")]
        public ActionResult EditOnGet(int? id)
        {
            Phone phone = db.Phones.Find(id);
            Customer customer = db.Customers.Single(c => c.ID == phone.Customer.ID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (phone == null)
            {
                return HttpNotFound();
            }

            ViewBag.CustomerID = customer.ID;

            return View(phone);
        }

        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOnPost([Bind(Include = "ID,PhoneContent,CustomerID")] Phone phone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phone).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Customer", new { id = Request.Form["CustomerID"] });
            }

            return View(phone);
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteOnGet(int? id)
        {
            Phone phone = db.Phones.Find(id);
            Customer customer = db.Customers.Single(c => c.ID == phone.Customer.ID);

            ViewBag.CustomerData = customer.FirstName + " " + customer.LastName + ", " + customer.Address;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (phone == null)
            {
                return HttpNotFound();
            }

            return View(phone);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOnPost(int id)
        {
            Phone phone = await db.Phones.FindAsync(id);
            var customer = await db.Customers.SingleAsync(c => c.ID == phone.Customer.ID);
            db.Phones.Remove(phone);
            await db.SaveChangesAsync();

            return RedirectToAction("Details", "Customer", new { id = customer.ID });
        }
    }
}