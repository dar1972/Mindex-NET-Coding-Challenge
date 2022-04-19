﻿using challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class CompensationDataSeeder
    {
        private CompensationContext _compensationContext;
        private const String SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public CompensationDataSeeder(CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
        }

        public async Task Seed()
        {
            if (!_compensationContext.Compensations.Any())
            {
                List<Compensation> compensaions = Load();
                _compensationContext.Compensations.AddRange(compensaions);

                await _compensationContext.SaveChangesAsync();
            }
        }

        private List<Compensation> Load()
        {
            using (FileStream fs = new FileStream(SEED_DATA_FILE, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Compensation> compensations = serializer.Deserialize<List<Compensation>>(jr);

                return compensations;
            }
        }
    }
}