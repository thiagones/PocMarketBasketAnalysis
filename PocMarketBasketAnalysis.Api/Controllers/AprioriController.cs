using Accord.MachineLearning.Rules;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace PocMarketBasketAnalysis.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AprioriController : ControllerBase
    {
        private readonly ILogger<AprioriController> _logger;

        public AprioriController(ILogger<AprioriController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string[][] dataset = {
                new string[] { "MILK","BREAD", "BISCUIT" },
                new string[] { "BREAD", "MILK", "BISCUIT", "CORNFLAKES" },
                new string[] { "BREAD", "TEA", "BOURNVITA"},
                new string[] { "JAM", "MAGGI", "BREAD", "MILK"},
                new string[] { "MAGGI", "TEA", "BISCUIT"},
                new string[] { "BREAD", "TEA", "BOURNVITA"},
                new string[] { "MAGGI", "TEA", "CORNFLAKES"},
                new string[] { "MAGGI", "BREAD", "TEA", "BISCUIT"},
                new string[] { "JAM", "MAGGI", "BREAD", "TEA"},
                new string[] { "BREAD", "MILK"},
                new string[] { "COFFEE", "COCK", "BISCUIT", "CORNFLAKES"},
                new string[] { "COFFEE", "COCK", "BISCUIT", "CORNFLAKES"},
                new string[] { "COFFEE", "SUGER", "BOURNVITA"},
                new string[] { "BREAD", "COFFEE", "COCK"},
                new string[] { "BREAD", "SUGER", "BISCUIT"},
                new string[] { "COFFEE", "SUGER", "CORNFLAKES"},
                new string[] { "BREAD", "SUGER", "BOURNVITA"},
                new string[] { "BREAD", "COFFEE", "SUGER"},
                new string[] { "BREAD", "COFFEE", "SUGER"},
                new string[] { "TEA", "MILK", "COFFEE", "CORNFLAKES"},
            };

            // Create a new A-priori learning algorithm with the requirements
            const decimal minSupport = 0.2m;
            var threshold = Convert.ToInt32(Math.Ceiling(dataset.Count() * minSupport));

            var apriori = new Apriori<string>(threshold: threshold, confidence: 0.5);

            // Use apriori to generate a n-itemset generation frequent pattern
            AssociationRuleMatcher<string> classifier = apriori.Learn(dataset);

            // Generate association rules from the itemsets:
            AssociationRule<string>[] rules = classifier.Rules;
            
            var result = rules.OrderByDescending(x => x.Confidence)
                .Select(x => x.ToString())
                .ToList();

            return Ok(result);
        }
    }
}
