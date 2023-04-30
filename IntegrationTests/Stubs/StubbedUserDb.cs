namespace IntegrationTests.Stubs;

public class StubbedUserDb : IUsersDb
{
    private readonly object _getStubbedData;
    private readonly object _getByForenameAndSurnameStubbedData;
    private readonly object _getByGuidStubbedData;
    private readonly object _insertStubbedData;

    public StubbedUserDb(
        object? getStubbedData = null,
        object? getByForenameAndSurnameStubbedData = null,
        object? getByGuidStubbedData = null,
        object? insertStubbedData = null)
    {
        _getStubbedData = getStubbedData ?? Task.FromResult<IList<User>?>(null);
        _getByForenameAndSurnameStubbedData = getByForenameAndSurnameStubbedData ?? Task.FromResult<IList<User>?>(null);
        _getByGuidStubbedData = getByGuidStubbedData ?? Task.FromResult<User?>(null);
        _insertStubbedData = insertStubbedData ?? Task.FromResult<User?>(null);
    }

    public Task<IList<User>?> Get() => (Task<IList<User>?>) _getStubbedData;

    public Task<IList<User>?> Get(string forename, string surname) => (Task<IList<User>?>)_getByForenameAndSurnameStubbedData;

    public Task<User?> Get(Guid guid) => (Task<User?>)_getByGuidStubbedData;

    public Task<User?> Insert(UserDto userDto) => (Task<User?>)_insertStubbedData; 
}
