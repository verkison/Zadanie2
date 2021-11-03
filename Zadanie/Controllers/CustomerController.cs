using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Models;
using Models.ApiModels;

namespace Controllers
{
    public class CustomerController : Controller
    {
        DBContext db = new DBContext();

        public async Task<ActionResult> Index(string searchBy, string search)
        {
            var customers = db.Customers.Include(c => c.CustomerStatus);

            switch (searchBy)
            {
                case "FirstName":
                    customers = customers.Where(c => c.FirstName.Contains(search));
                    break;
                case "LastName":
                    customers = customers.Where(c => c.LastName.Contains(search));
                    break;
                case "Address":
                    customers = customers.Where(c => c.Address.Contains(search));
                    break;

                case "Status1":
                    customers = customers.Where(c => c.CustomerStatus.Status == "Potencjalny");
                    break;
                case "Status2":
                    customers = customers.Where(c => c.CustomerStatus.Status == "Obecny");
                    break;

                case "Opcja1":
                    customers = customers.Where(c => c.Emails.Count() == 0 && c.Phones.Count() == 0);
                    break;
                case "Opcja2":
                    customers = customers.Where(c => c.Emails.Count() > 1);
                    break;

                default:
                    break;
            }

            return View(await customers.ToListAsync());
        }

        [HttpGet, ActionName("Create")]
        public ActionResult CreateOnGet()
        {
            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status");

            return View();
        }

        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOnPost(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Customer", null);
            }

            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status", customer.StatusID);
            return View(customer);
        }

        [HttpGet, ActionName("Details")]
        public ActionResult DetailsOnGet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpGet, ActionName("Edit")]
        public ActionResult EditOnGet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status", customer.StatusID);
            return View(customer);
        }

        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOnPost(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", "Customer", null);
            }

            ViewBag.StatusID = new SelectList(db.CustomerStatus, "ID", "Status", customer.StatusID);
            return View(customer);
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteOnGet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = db.Customers.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOnPost(int id)
        {
            var customer = await db.Customers.FindAsync(id);
            var emails = db.Emails.Include(e => e.Customer);
            var phones = db.Phones.Include(e => e.Customer);

            {
                foreach (var e in emails)
                {
                    if (e.Customer.ID == id)
                        db.Emails.Remove(e);
                }

                foreach (var p in phones)
                {
                    if (p.Customer.ID == id)
                        db.Phones.Remove(p);
                }

                await db .SaveChangesAsync();
            }

            db.Customers.Remove(customer);
            await db .SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("SendingEmail")]
        public ActionResult SendingEmailOnGet(string mailTo, int customerID)
        {
            ViewBag.MailTo = mailTo;

            EmailToSend ets = new EmailToSend();
            ets.EmailTo = mailTo;

            ViewBag.CustomerID = customerID;

            return View(ets);
        }

        [HttpPost, ActionName("SendingEmail")]
        public ActionResult SendingEmailOnPost(EmailToSend ets)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri("https://localhost:44389/api/EmailAPI");

            var consumewebapi = hc.PostAsJsonAsync("EmailAPI", ets);

            consumewebapi.Wait();

            var sendEmail = consumewebapi.Result;
            if (sendEmail.IsSuccessStatusCode)
            {
                ViewBag.message = "Email został wysłany";
            }
            else
                ViewBag.message = sendEmail.ReasonPhrase;

            return View();
        }
    }
}