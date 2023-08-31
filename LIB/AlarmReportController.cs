using DbExtensions;
using PHD_TAS_LIB.db;
using PHD_TAS_LIB.entity.report;
using PHD_TAS_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHD_TAS_WEB.Controllers
{
    public class AlarmReportController : Controller
    {
        // GET: AlarmReport
        public ActionResult Index()
        {
            var db = DBFactory.GetEnsureOpen();
            var filter = new AlarmReportFilter();
            filter.dateBegin.check = true;
            ViewBag.filter = filter;

            var alarms = db.Table<AlarmReport>().Include("bay")
                .Where("datetime BETWEEN {0} AND {1}", filter.dateBegin.value, filter.dateEnd.AddDays(1)).OrderBy("datetime DESC").ToArray();

            ViewBag.bays = db.From("bay").Select("id,name").ToArray();

            db.Connection.Close();
            return View(alarms);
        }

        [HttpGet]
        public ActionResult Filter()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(AlarmReportFilter filter)
        {

            if (filter == null)
                return RedirectToAction("Index");

            ViewBag.filter = filter;

            var db = DBFactory.GetEnsureOpen();
            var reports = db.Table<AlarmReport>().Include("bay");
            //TODO: Create extensions methods for dynamic where creation
            var where = new SqlBuilder();
            if (filter.dateBegin.check)
            {
                where.AppendToCurrentClause("datetime BETWEEN {0} AND {1}", filter.dateBegin.value, filter.dateEnd.AddDays(1));
            }
            if (filter.name.check)
            {
                if (!where.IsEmpty)
                    where.CurrentSeparator = " AND ";
                where.AppendToCurrentClause("name LIKE {0}", "%"+ filter.name.value+ "%");
            }
            if (filter.idBay.check)
            {
                if (!where.IsEmpty)
                    where.CurrentSeparator = " AND ";
                where.AppendToCurrentClause("idBay = {0}", filter.idBay.value);
            }
            var operations = where.IsEmpty ? reports.OrderBy("datetime DESC").ToArray() : reports.Where(where.ToString(), where.ParameterValues.ToArray()).OrderBy("datetime DESC").ToArray();
            ViewBag.bays = db.From("bay").Select("id,name").ToArray();
            db.Connection.Close();
            return View("Index", operations);
        }
    }
}