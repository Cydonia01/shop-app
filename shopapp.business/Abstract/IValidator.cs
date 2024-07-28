/*
* This interface is used to validate the entities.
*/
namespace shopapp.business.Abstract
{
    public interface IValidator<T>
    {
        string ErrorMessage { get; set; }
        bool Validate(T entity);
    }
}