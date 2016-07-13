using Newtonsoft.Json.Serialization;
using NServiceBus.MessageInterfaces;
using System;
using System.Linq;

namespace NServiceBus.Newtonsoft.Json
{
    internal class MessageContractResolver : DefaultContractResolver
    {
        private readonly IMessageMapper _mapper;
        public MessageContractResolver(IMessageMapper messageMapper)
        {
            this._mapper = messageMapper;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var mappedTypeFor = _mapper.GetMappedTypeFor(objectType);

            if (mappedTypeFor == null)
                return base.CreateObjectContract(objectType);

            var jsonContract = base.CreateObjectContract(mappedTypeFor);
            jsonContract.DefaultCreator = () => _mapper.CreateInstance(mappedTypeFor);

            return jsonContract;
        }
    }
}