using EmployeeService.ClientHttp.Interfaces;
using EmployeeService.Shared.DTOs;
using System.Net.Http.Json;

namespace EmployeeService.ClientHttp.Implementations
{
    /// <summary>
    /// Implementazione del client HTTP per l'interazione con il servizio EmployeeService.
    /// </summary>
    /// <remarks>
    /// Questa classe utilizza <see cref="HttpClient"/> per comunicare con le API REST
    /// del servizio di gestione dipendenti.
    /// </remarks>
    public class EmployeeClient : IEmployeeClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inizializza una nuova istanza del client HTTP.
        /// </summary>
        /// <param name="httpClient">
        /// Istanza di <see cref="HttpClient"/> configurata tramite dependency injection.
        /// </param>
        public EmployeeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EmployeeDto>?> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeDto>>("api/employees");
        }

        /// <inheritdoc/>
        public async Task<EmployeeDto?> GetEmployeeByIdAsync(short id)
        {
            return await _httpClient.GetFromJsonAsync<EmployeeDto>($"api/employees/{id}");
        }

        /// <inheritdoc/>
        public async Task<bool> GetEmployeeEligibilityAsync(short id)
        {
            return await _httpClient.GetFromJsonAsync<bool>($"api/employees/{id}/eligibility");
        }

        /// <inheritdoc/>
        public async Task<short?> CreateEmployeeAsync(CreateEmployeeDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employees", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<short>();
            }

            return null;
        }
    }
}