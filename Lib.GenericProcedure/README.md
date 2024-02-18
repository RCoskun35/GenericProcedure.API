# Lib.GenericProcedure
If a result set is needed after complex database operations in AspNetCore projects, this process can be accomplished by making a stored procedure call, serializing the desired result into JSON using the @response output parameter, and then deserializing the result into a passed generic class in the back-end.
It offers a more efficient solution instead of the DbSet<T> method.
