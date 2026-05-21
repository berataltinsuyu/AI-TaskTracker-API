## 2025-02-27 - [Stop Leaking Exception Messages to Client]
**Vulnerability:** Information Disclosure in `GlobalExceptionMiddleware.cs`
**Learning:** The middleware was catching all exceptions and passing `ex.Message` directly to the API response. This could potentially leak internal system details, stack traces, or sensitive information from external APIs (like the Gemini API) to any user when an unexpected error occurred.
**Prevention:** In production environments, generic error messages (e.g., "An internal server error occurred.") should be returned to the client. Detailed error messages should only be logged internally and/or exposed exclusively in development environments.
