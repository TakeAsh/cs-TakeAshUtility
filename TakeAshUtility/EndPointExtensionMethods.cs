using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TakeAshUtility {

    public static class EndPointExtensionMethods {

        public static bool EqualsIgnoreScopeId(this EndPoint self, EndPoint other) {
            if (Object.ReferenceEquals(self, other)) {
                return true;
            }
            if ((object)self == null || (object)other == null) {
                return false;
            }

            var ipepSelf = self as IPEndPoint;
            var ipepOther = other as IPEndPoint;
            return ipepSelf == null || ipepOther == null ?
                self.Equals(other) :
                ipepSelf.Port == ipepOther.Port &&
                    ipepSelf.Address
                        .GetAddressBytes()
                        .SequenceEqual(ipepOther.Address
                            .GetAddressBytes());
        }
    }
}
