using DummyUIApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace DummyUIApp.Controllers
{
    public class SignupController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SignupController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(SignupViewModel model)
        {
            // Handle submission here
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = _httpClientFactory.CreateClient();

            var etnaRequest = new
            {
                route = "ETNA",
                content = new
                {
                    Credentials = new { model.Login, model.Email, model.Password },
                    Name = new { model.FirstName, model.LastName, model.MiddleName, model.Suffix },
                    model.BrokerUsername
                }
            };

            var rqdRequest = new
            {
                route = "RQD",
                content = new
                {
                    corr = "TPRO",
                    office = "001",
                    acct_no = "",
                    acct_type = "C",
                    acct_name = $"{model.FirstName} {model.LastName}",
                    designator = "C",
                    ssn = model.SSN,
                    ssn_type = model.SSNType,
                    cost_desig = "L",
                    legal_entity = "INDIV",
                    short_name = model.FirstName,
                    salute = model.Salutation,
                    contactf = model.FirstName,
                    m_initial = model.MiddleName,
                    contactl = model.LastName,
                    day_trader = model.DayTrader,
                    capacity = model.Capacity,
                    sol_unsol = model.SolUnsol,
                    phone = model.Phone,
                    fax = model.Fax,
                    citizenship = model.Citizenship,
                    dob = model.DOB.ToString("MM/dd/yyyy"),
                    email = model.Email,
                    rr_cd = "",
                    street_num = model.StreetNum,
                    street_name = model.StreetName,
                    street_type = model.StreetType,
                    unit_num = model.UnitNum,
                    consolidated_address = $"{model.StreetNum} {model.StreetName}",
                    city = model.City,
                    st_cd = model.StateCode,
                    zip = model.Zip,
                    country = model.Country,
                    sec_charge = "Y",
                    employee = "Y",
                    marital_status = model.MaritalStatus,
                    dependants = model.Dependants,
                    nasd_emp = "N",
                    nasd_spouse = "N",
                    employer = model.Employer,
                    inv_objective = model.InvestmentObjective,
                    risk_tolerence = model.RiskTolerance,
                    annual_inc = model.AnnualIncome,
                    net_worth_liq = model.NetWorthLiquid,
                    net_worth = model.NetWorth,
                    level_stocks = model.LevelStocks,
                    years_stocks = model.YearsStocks,
                    level_options = model.LevelOptions,
                    years_options = model.YearsOptions,
                    dir_shr_dec = model.DirShrDec,
                    dir_shr_dec_dtl = model.DirShrDecDtl,
                    cpa_name = model.CPAName,
                    cpa_addr1 = model.CPAAddr1,
                    cpa_phone = model.CPAPhone,
                    cpa_email = model.CPAEmail,
                    contact_cpa = "N",
                    att_name = model.AttorneyName,
                    att_addr1 = model.AttorneyAddr1,
                    att_phone = model.AttorneyPhone,
                    att_email = model.AttorneyEmail,
                    contact_att = "Y",
                    id_type = model.IdType,
                    id_num = model.IdNum,
                    id_place = model.IdPlace,
                    avg_price = "Y",
                    prg_trading = "Y",
                    index_arbitrage = "Y",
                    jt_mnr_dob = model.JointMinorDOB.ToString("MM/dd/yyyy"),
                    jt_mnr_citizen = model.JointMinorCitizenship,
                    jt_mnr_relation = model.JointMinorRelation,
                    time_horizon = model.TimeHorizon,
                    liq_needs = model.LiqNeeds,
                    div_reinv = model.DivReinv,
                    tax_lot = model.TaxLot,
                    optn_level = "1",
                    currency = "USD",
                    stmnt_mail = "E",
                    cnfrm_mail = "E",
                    ira_agreement_version = "1.0",
                    ira_agreement_timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                }
            };

            var apiUrl = "https://gmh-apim-uat.azure-api.net/v1/userregistrationroute/";
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var etnaPayload = JsonSerializer.Serialize(etnaRequest, options);
            var rqdPayload = JsonSerializer.Serialize(rqdRequest, options);

            var etnaResponse = await client.PostAsync(apiUrl,
                new StringContent(etnaPayload, Encoding.UTF8, "application/json"));
            var rqdResponse = await client.PostAsync(apiUrl,
                new StringContent(rqdPayload, Encoding.UTF8, "application/json"));

            if (etnaResponse.IsSuccessStatusCode && rqdResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Registration failed. Check details and try again.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}

namespace DummyUIApp.Models
{
    public class SignupViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Login { get; set; }
        public string BrokerUsername { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Salutation { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public DateTime DOB { get; set; }

        public string StreetNum { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string UnitNum { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public string IdType { get; set; }
        public string IdNum { get; set; }
        public string IdPlace { get; set; }

        public string SSN { get; set; }
        public string SSNType { get; set; }

        public string MaritalStatus { get; set; }
        public int Dependants { get; set; }
        public string Citizenship { get; set; }
        public string Employer { get; set; }
        public string InvestmentObjective { get; set; }
        public string RiskTolerance { get; set; }
        public int AnnualIncome { get; set; }
        public int NetWorth { get; set; }
        public int NetWorthLiquid { get; set; }

        public DateTime JointMinorDOB { get; set; }
        public string JointMinorCitizenship { get; set; }
        public string JointMinorRelation { get; set; }

        public string CPAName { get; set; }
        public string CPAPhone { get; set; }
        public string CPAEmail { get; set; }
        public string CPAAddr1 { get; set; }

        public string AttorneyName { get; set; }
        public string AttorneyPhone { get; set; }
        public string AttorneyEmail { get; set; }
        public string AttorneyAddr1 { get; set; }

        public string DayTrader { get; set; }
        public string Capacity { get; set; }
        public string SolUnsol { get; set; }

        public string DirShrDec { get; set; }
        public string DirShrDecDtl { get; set; }
        public string DivReinv { get; set; }
        public string TaxLot { get; set; }

        public string LevelStocks { get; set; }
        public int YearsStocks { get; set; }
        public string LevelOptions { get; set; }
        public int YearsOptions { get; set; }
        public string LevelFixedInc { get; set; }
        public int YearsFixedInc { get; set; }

        public string TimeHorizon { get; set; }
        public string LiqNeeds { get; set; }
    }
}

