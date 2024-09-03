namespace Hospital.Core.Exceptions;

public class NotFoundException(string name) : BaseException(404, $"{name} not found");