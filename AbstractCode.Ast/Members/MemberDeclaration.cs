// This file is part of AbstractCode.
// 
// AbstractCode is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AbstractCode is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AbstractCode.  If not, see <http://www.gnu.org/licenses/>.
// 

namespace AbstractCode.Ast.Members
{
    public abstract class MemberDeclaration : AstNode, IModifierProvider, ICustomAttributeProvider
    {
        protected MemberDeclaration()
        {
            ModifierElements = new AstNodeCollection<ModifierElement>(this, AstNodeTitles.Modifier);
            CustomAttributeSections = new AstNodeCollection<CustomAttributeSection>(this, AstNodeTitles.CustomAttributeSection);
        }

        public Modifier Modifiers
        {
            get
            {
                var modifiers = Modifier.None;
                foreach (var element in ModifierElements)
                    modifiers |= element.Modifier;
                return modifiers;
            }
        }

        public AstNodeCollection<ModifierElement> ModifierElements
        {
            get;
            private set;
        }

        public bool IsPrivate
        {
            get { return Modifiers.HasMaskedFlag(Modifier.VisibilityMask, Modifier.Private); }
        }

        public bool IsInternal
        {
            get { return Modifiers.HasMaskedFlag(Modifier.VisibilityMask, Modifier.Internal); }
        }

        public bool IsProtected
        {
            get { return Modifiers.HasMaskedFlag(Modifier.VisibilityMask, Modifier.Protected); }
        }

        public bool IsPublic
        {
            get { return Modifiers.HasMaskedFlag(Modifier.VisibilityMask, Modifier.Public); }
        }
                
        public AstNodeCollection<CustomAttributeSection> CustomAttributeSections
        {
            get;
        }

        protected bool MatchModifiersAndAttributes(MemberDeclaration other)
        {
            return Modifiers == other.Modifiers && CustomAttributeSections.Match(other.CustomAttributeSections);
        }
    }
}
