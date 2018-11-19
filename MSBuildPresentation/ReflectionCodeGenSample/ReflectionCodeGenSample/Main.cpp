#include "Includes.h"

Reflection Reflection::_singleton = Reflection();

Reflect
void func1()
{
	printf("Called func1\n");
}

Reflect
void func2()
{
	printf("Called func2\n");
}

Reflect
void func3()
{
	printf("Called func3\n");
}

Reflect
void func4()
{
	printf("Called func4\n");
}

int main()
{
	ReflectRegisteredFunctions();
	printf("printing all function names\n");
	Reflection::PrintAll();
	printf("calling functions by name\n");
	Reflection::CallFunction("func1");
	Reflection::CallFunction("func2");
	Reflection::CallFunction("func3");
	Reflection::CallFunction("func4");
	getchar();

	return 0;
}