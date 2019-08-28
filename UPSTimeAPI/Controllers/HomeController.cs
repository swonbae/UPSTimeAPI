using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPSTimeAPI.Models;
using UPSTimeAPI.ServiceReference3;

namespace UPSTimeAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TimeInTransitService tntService = new TimeInTransitService();
            TimeInTransitRequest tntRequest = new TimeInTransitRequest();
            RequestType request = new RequestType();
            String[] requestOption = { "TNT" };
            request.RequestOption = requestOption;
            tntRequest.Request = request;

            RequestShipFromType shipFrom = new RequestShipFromType();
            RequestShipFromAddressType addressFrom = new RequestShipFromAddressType();
            addressFrom.City = "Toronto";
            addressFrom.CountryCode = "CA";
            addressFrom.PostalCode = "M1P4P5";
            //addressFrom.StateProvinceCode = "ShipFrom state province code";
            shipFrom.Address = addressFrom;
            tntRequest.ShipFrom = shipFrom;

            RequestShipToType shipTo = new RequestShipToType();
            RequestShipToAddressType addressTo = new RequestShipToAddressType();
            addressTo.City = "Toronto";
            addressTo.CountryCode = "CA";
            addressTo.PostalCode = "M1P4P5";
            //addressTo.StateProvinceCode = "ShipTo state province code";
            shipTo.Address = addressTo;
            tntRequest.ShipTo = shipTo;

            PickupType pickup = new PickupType();
            pickup.Date = "20190830";
            tntRequest.Pickup = pickup;

            //Below code uses dummy data for reference. Please update as required.
            ShipmentWeightType shipmentWeight = new ShipmentWeightType();
            shipmentWeight.Weight = "10";
            CodeDescriptionType unitOfMeasurement = new CodeDescriptionType();
            unitOfMeasurement.Code = "KGS";
            unitOfMeasurement.Description = "Kilograms";
            shipmentWeight.UnitOfMeasurement = unitOfMeasurement;
            tntRequest.ShipmentWeight = shipmentWeight;

            tntRequest.TotalPackagesInShipment = "1";
            InvoiceLineTotalType invoiceLineTotal = new InvoiceLineTotalType();
            invoiceLineTotal.CurrencyCode = "CAD";
            invoiceLineTotal.MonetaryValue = "10";
            tntRequest.InvoiceLineTotal = invoiceLineTotal;
            tntRequest.MaximumListSize = "1";

            UPSSecurity upss = new UPSSecurity();
            UPSSecurityServiceAccessToken upsSvcToken = new UPSSecurityServiceAccessToken();
            upsSvcToken.AccessLicenseNumber = "3D6A1DD5F39023B5";
            upss.ServiceAccessToken = upsSvcToken;
            UPSSecurityUsernameToken upsSecUsrnameToken = new UPSSecurityUsernameToken();
            upsSecUsrnameToken.Username = "Deverloper2019";
            upsSecUsrnameToken.Password = "Deverloper=2019";
            upss.UsernameToken = upsSecUsrnameToken;
            tntService.UPSSecurityValue = upss;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11; //This line will ensure the latest security protocol for consuming the web service call.
            Console.WriteLine(tntRequest);
            TimeInTransitResponse tntResponse = tntService.ProcessTimeInTransit(tntRequest);

            Console.WriteLine("Response code: " + tntResponse.Response.ResponseStatus.Code);
            Console.WriteLine("Response description: " + tntResponse.Response.ResponseStatus.Description);

            TransitResponseType responseItem = (TransitResponseType)tntResponse.Item;

            RatePackage model = new RatePackage()
            {
                timeResponse = responseItem
            };
            
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}