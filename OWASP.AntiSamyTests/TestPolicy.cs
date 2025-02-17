﻿/*
 * Copyright (c) 2022, Sebastián Passaro
 * 
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * Neither the name of OWASP nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Collections.Generic;
using OWASP.AntiSamy.Html;
using OWASP.AntiSamy.Html.Model;

namespace AntiSamyTests
{
    internal class TestPolicy : InternalPolicy
    {
        public TestPolicy(ParseContext parseContext) : base(parseContext) 
        { 
            // No specific logic
        }

        public TestPolicy(Policy old, Dictionary<string, string> directives, Dictionary<string, Tag> tagRules, Dictionary<string, Property> cssProperties) 
            : base(old, directives, tagRules, cssProperties)
        {
            // No specific logic
        }

        public static new TestPolicy GetInstance(string filename) => GetInternalPolicyFromFile(filename);

        internal static TestPolicy GetInternalPolicyFromFile(string filename)
        {
            return new TestPolicy(GetParseContext(GetXmlDocumentFromFile(filename)));
        }

        internal new TestPolicy CloneWithDirective(string name, string value)
        {
            var newDirectives = new Dictionary<string, string>(directives);

            if (newDirectives.ContainsKey(name))
            {
                newDirectives[name] = value;
            }
            else
            {
                newDirectives.Add(name, value);
            }

            return new TestPolicy(this, newDirectives, tagRules, cssRules);
        }

        internal TestPolicy MutateTag(Tag tag)
        {
            var newTagRules = new Dictionary<string, Tag>(tagRules);
            string tagNameToLower = tag.Name.ToLowerInvariant();

            if (newTagRules.ContainsKey(tagNameToLower))
            {
                newTagRules[tagNameToLower] = tag;
            }
            else
            {
                newTagRules.Add(tagNameToLower, tag);
            }

            return new TestPolicy(this, directives, newTagRules, cssRules);
        }

        internal TestPolicy AddCssProperty(Property cssProperty)
        {
            var newCssProperties = new Dictionary<string, Property>(cssRules)
            {
                { cssProperty.Name.ToLowerInvariant(), cssProperty }
            };
            return new TestPolicy(this, directives, tagRules, newCssProperties);
        }
    }
}
