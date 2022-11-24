using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using administracionCliente_Mvc.Models;
using Newtonsoft.Json;

namespace administracionCliente_Mvc.Controllers
{
    public class EmpresaController : Controller
    {
        private DasGlobalEntities db = new DasGlobalEntities();

        // GET: Empresa
        public async Task<ActionResult> Index()
        {

            var httpCliente = new HttpClient();
            var json = await httpCliente.GetStringAsync("https://localhost:7074/empresa");
            var empresaList = JsonConvert.DeserializeObject<List<Empresa>>(json);

            return View(empresaList);
        }

        public async Task<Empresa> encontrar(int? id) {
            var httpCliente = new HttpClient();
            var json = await httpCliente.GetStringAsync($"https://localhost:7074/empresa/{id}");
            var empresa = JsonConvert.DeserializeObject<Empresa>(json);
            return empresa;
        }


        // GET: Empresa/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa =  await encontrar(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // GET: Empresa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmpresaNumero,Nombre,pais")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                
                var httpCliente = new HttpClient();
                var empresaSerializable = JsonConvert.SerializeObject(empresa);
                var content = new StringContent(empresaSerializable,Encoding.UTF8,"application/json");
                var json = await httpCliente.PostAsync("https://localhost:7074/empresa",content);

                //db.Empresa.Add(empresa);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        // GET: Empresa/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = await encontrar(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmpresaNumero,Nombre,pais")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                var httpCliente = new HttpClient();
                var empresaSerializable = JsonConvert.SerializeObject(empresa);
                var content = new StringContent(empresaSerializable, Encoding.UTF8, "application/json");
                var json = await httpCliente.PutAsync($"https://localhost:7074/empresa/{empresa.EmpresaNumero}", content);

                //db.Empresa.Add(empresa);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        // GET: Empresa/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = await encontrar(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Empresa empresa = await encontrar(id);

            var httpCliente = new HttpClient();
            var json = await httpCliente.DeleteAsync($"https://localhost:7074/empresa/{id}");

            //db.Empresa.Remove(empresa);
            //db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
