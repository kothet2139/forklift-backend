using ForkliftControlSystem.Domain.Exceptions;

namespace ForkliftControlSystem.Domain.Utilities;

public static class ValidationHelper
{
    public static void ValidateDegrees(int value, char cmd)
    {
        if (value <= 0 || value > 360)
            throw new BadRequestException($"Invalid degree value after '{cmd}': must be between 0 and 360");

        if (value % 90 != 0)
            throw new BadRequestException($"Invalid degree value after '{cmd}': must be a multiple of 90");
    }
}
