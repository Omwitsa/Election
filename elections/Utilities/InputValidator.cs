using elections.ResponseModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace elections.Utilities
{
	public class InputValidator
	{
		private Logger logger = LogManager.GetLogger("fileLogger");
		public ReturnData ValidateInputs(List<Tuple<string, string, DataType>> requiredFields)
		{
			foreach (Tuple<string, string, DataType> tuple in requiredFields)
			{
				var dataType = tuple.Item3;
				var value = tuple.Item2;
				var error = $"{tuple.Item1} is required";

				if (string.IsNullOrEmpty(tuple.Item2))
					return new ReturnData
					{
						Success = false,
						Message = error
					};

				switch (dataType)
				{
					case DataType.Integer:
						var validInt = ParseInt(value, tuple);
						if (!validInt.Success)
							return validInt;
						break;

					case DataType.Decimal:
						var validDecimal = ParseDecimal(value, tuple);
						if (!validDecimal.Success)
							return validDecimal;
						break;

					case DataType.Float:
						var validFloat = ParseFloat(value, tuple);
						if (!validFloat.Success)
							return validFloat;
						break; 

					case DataType.Email:
						var validEmail = CheckEmail(value, tuple);
						if (!validEmail.Success)
							return validEmail;
						break;

					case DataType.Password:
						var validPassword = CheckPassword(value, tuple);
						if (!validPassword.Success)
							return validPassword;
						break;
				}
			}
				
			return new ReturnData
			{
				Success = true,
			};
		}

		private ReturnData ParseInt(string value, Tuple<string, string, DataType> tuple)
		{
			try
			{
				if (int.Parse(value).GetType() != typeof(int))
					return new ReturnData
					{
						Success = false,
						Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
					};
				return new ReturnData
				{
					Success = true
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t InputValidatorParseIntError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
				};
			}
		}

		private ReturnData ParseDecimal(string value, Tuple<string, string, DataType> tuple)
		{
			try
			{
				if (decimal.Parse(value).GetType() != typeof(decimal))
					return new ReturnData
					{
						Success = false,
						Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
					};

				return new ReturnData
				{
					Success = true
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t InputValidatorParseDecimalError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
				};
			}
		}

		private ReturnData ParseFloat(string value, Tuple<string, string, DataType> tuple)
		{
			try
			{
				if (decimal.Parse(value).GetType() != typeof(decimal))
					return new ReturnData
					{
						Success = false,
						Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
					};

				return new ReturnData
				{
					Success = true
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t InputValidatorParseFloatError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
				};
			}
		}

		private ReturnData CheckEmail(string value, Tuple<string, string, DataType> tuple)
		{
			if (!Regex.IsMatch(value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
				return new ReturnData
				{
					Success = false,
					Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
				};

			return new ReturnData
			{
				Success = true
			};
		}

		private ReturnData CheckPassword(string value, Tuple<string, string, DataType> tuple)
		{
			try
			{
				value = value ?? "null";
				if (value.Length < 6)
					return new ReturnData
					{
						Success = false,
						Message = $"{tuple.Item1} must be atleast 6 characters"
					};

				return new ReturnData
				{
					Success = true
				};
			}
			catch (Exception ex)
			{
				logger.Error($"\t InputValidatorCheckPasswordError: \t {ex}");
				return new ReturnData
				{
					Success = false,
					Message = $"{tuple.Item1} is not of datatype {tuple.Item3}"
				};
			}
		}
	}

	public enum DataType
	{
		Default = 0,
		Integer = 1,
		Decimal = 2,
		Float = 3,
		Email = 4,
		Password = 5
	}
}
