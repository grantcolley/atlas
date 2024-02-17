using Atlas.Blazor.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Atlas.Blazor.Core.Render
{
    public abstract class ModelRender<T> where T : class, new()
    {
        private readonly List<PropertyRender<T>> _propertyRenders = new();
        private readonly List<Container> _renderContainers = new();

        public abstract void Configure();

        protected IPropertyRenderBuilder<T> RenderProperty<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            MemberInfo? memberInfo = expression.GetMemberInfo();

            if (memberInfo == null) throw new NullReferenceException(nameof(memberInfo));

            PropertyRender<T> propertyRender = new(memberInfo.Name, this);

            _propertyRenders.Add(propertyRender);

            return new PropertyRenderBuilder<T>(propertyRender);
        }

        protected IPropertyRenderBuilder<T> RenderGenericProperty<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            MemberInfo? memberInfo = expression.GetMemberInfo();

            if (memberInfo == null) throw new NullReferenceException(nameof(memberInfo));

            PropertyRender<T> propertyRender = new(memberInfo.Name, this);

            propertyRender.GenericType = expression.ReturnType;

            _propertyRenders.Add(propertyRender);

            return new PropertyRenderBuilder<T>(propertyRender);
        }

        public void RenderContainer(Container renderContainers)
        {
            if (renderContainers == null) throw new ArgumentNullException(nameof(renderContainers));
            if (renderContainers.ContainerCode == null) throw new NullReferenceException(nameof(renderContainers.ContainerCode));
            if (renderContainers.Title == null) throw new NullReferenceException(nameof(renderContainers.Title));

            _renderContainers.Add(renderContainers);
        }

        public IEnumerable<ModelPropertyRender<T>> GetModelPropertyRenders(T model)
        {
            List<ModelPropertyRender<T>> modelPropertyRenders = new();

            foreach (PropertyRender<T> propertyRender in _propertyRenders)
            {
                modelPropertyRenders.Add(new ModelPropertyRender<T>(model, propertyRender));
            }

            return modelPropertyRenders;
        }

        public IEnumerable<ModelPropertyRender<T>> GetContainerPropertyRenders(string containerCode, T model)
        {
            if (containerCode == null) throw new ArgumentNullException(nameof(containerCode));

            List<ModelPropertyRender<T>> modelPropertyRenders = new();

            IEnumerable<PropertyRender<T>> propertyRenders = _propertyRenders.Where(
                p => !string.IsNullOrWhiteSpace(p.ContainerCode) && p.ContainerCode.Equals(containerCode));

            foreach (PropertyRender<T> propertyRender in propertyRenders)
            {
                modelPropertyRenders.Add(new ModelPropertyRender<T>(model, propertyRender));
            }

            return modelPropertyRenders;
        }

        public IEnumerable<Container> GetContainers()
        {
            return _renderContainers.OrderBy(c => c.Order);
        }

        public bool ContainerExists(string containerCode)
        {
            return _renderContainers.Exists(c => !string.IsNullOrWhiteSpace(c.ContainerCode)
                                                && c.ContainerCode.Equals(containerCode));
        }
    }
}
