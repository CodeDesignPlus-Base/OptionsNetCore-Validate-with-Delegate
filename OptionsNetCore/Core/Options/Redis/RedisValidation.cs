using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OptionsNetCore.Core.Options.Redis
{
    public class RedisValidation : IValidateOptions<RedisOptions>
    {
        private readonly RedisOptions _config;

        public RedisValidation(IConfiguration config)
        {
            this._config = config
                .GetSection(RedisOptions.Redis)
                .Get<RedisOptions>();
        }

        public ValidateOptionsResult Validate(string name, RedisOptions options)
        {
            string failures = null;

            if (options.ConnectRetry > 3 && options.ConnectTimeout < 5000)
                failures = "ConnectTimeout must be > than 5000";

            if (string.IsNullOrEmpty(failures))
                return ValidateOptionsResult.Fail(failures);

            return ValidateOptionsResult.Success;
        }
    }
}
