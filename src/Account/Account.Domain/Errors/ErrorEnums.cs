namespace Account.Domain.Errors;

public enum ErrorLayer { Domain, Application, Infrastructure, Api }

public enum ErrorSeverity { Information, Warning, Error, Critical }

public enum ErrorExposure { LogOnly, Response }