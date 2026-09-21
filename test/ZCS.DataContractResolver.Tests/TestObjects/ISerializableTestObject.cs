#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Tests.TestObjects.Organization;

namespace Newtonsoft.Json.Tests.TestObjects
{
    [Serializable]
    public class ISerializableTestObject : ISerializable
    {
        internal readonly string _stringValue;
        internal readonly int _intValue;
        internal readonly DateTimeOffset _dateTimeOffsetValue;
        internal readonly Person _personValue;
        internal readonly Person _nullPersonValue;
        internal readonly int? _nullableInt;
        internal readonly bool _booleanValue;
        internal readonly byte _byteValue;
        internal readonly char _charValue;
        internal readonly DateTime _dateTimeValue;
        internal readonly decimal _decimalValue;
        internal readonly short _shortValue;
        internal readonly long _longValue;
        internal readonly sbyte _sbyteValue;
        internal readonly float _floatValue;
        internal readonly ushort _ushortValue;
        internal readonly uint _uintValue;
        internal readonly ulong _ulongValue;

        public ISerializableTestObject(string stringValue, int intValue, DateTimeOffset dateTimeOffset, Person personValue)
        {
            _stringValue = stringValue;
            _intValue = intValue;
            _dateTimeOffsetValue = dateTimeOffset;
            _personValue = personValue;
            _dateTimeValue = new DateTime(0, DateTimeKind.Utc);
        }

        protected ISerializableTestObject(SerializationInfo info, StreamingContext context)
        {
            _stringValue = info.GetString("stringValue");
            _intValue = info.GetInt32("intValue");
            _dateTimeOffsetValue = (DateTimeOffset)info.GetValue("dateTimeOffsetValue", typeof(DateTimeOffset));
            _personValue = (Person)info.GetValue("personValue", typeof(Person));
            _nullPersonValue = (Person)info.GetValue("nullPersonValue", typeof(Person));
            _nullableInt = (int?)info.GetValue("nullableInt", typeof(int?));

            _booleanValue = info.GetBoolean("booleanValue");
            _byteValue = info.GetByte("byteValue");
            _charValue = info.GetChar("charValue");
            _dateTimeValue = info.GetDateTime("dateTimeValue");
            _decimalValue = info.GetDecimal("decimalValue");
            _shortValue = info.GetInt16("shortValue");
            _longValue = info.GetInt64("longValue");
            _sbyteValue = info.GetSByte("sbyteValue");
            _floatValue = info.GetSingle("floatValue");
            _ushortValue = info.GetUInt16("ushortValue");
            _uintValue = info.GetUInt32("uintValue");
            _ulongValue = info.GetUInt64("ulongValue");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stringValue", (object)_stringValue);
            info.AddValue("intValue", _intValue);
            info.AddValue("dateTimeOffsetValue", (object)_dateTimeOffsetValue);
            info.AddValue("personValue", (object)_personValue);
            info.AddValue("nullPersonValue", (object)_nullPersonValue);
            info.AddValue("nullableInt", null);

            info.AddValue("booleanValue", _booleanValue);
            info.AddValue("byteValue", _byteValue);
            info.AddValue("charValue", _charValue);
            info.AddValue("dateTimeValue", _dateTimeValue);
            info.AddValue("decimalValue", _decimalValue);
            info.AddValue("shortValue", _shortValue);
            info.AddValue("longValue", _longValue);
            info.AddValue("sbyteValue", _sbyteValue);
            info.AddValue("floatValue", _floatValue);
            info.AddValue("ushortValue", _ushortValue);
            info.AddValue("uintValue", _uintValue);
            info.AddValue("ulongValue", _ulongValue);
        }
    }
}