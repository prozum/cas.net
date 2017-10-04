nunit2_dep = dependency('nunit2')

ast_tests = executable('Ast.Tests', 
                      'Ast.Tests.cs', 
                      link_with: ast_lib,
                      dependencies: nunit2_dep)
test('Ast.Tests', ast_tests)
