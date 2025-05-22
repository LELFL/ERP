# ERP ��Ŀ README

## ��Ŀ����
����Ŀ��һ������ .NET Aspire ��������ԭ��΢����Ӧ�ã������� Clean Architecture �� CQRS ģʽ����ʵ��ģ�黯����ά���Ϳ���չ��ϵͳ�ܹ��������֤����ʹ�� OpenIddict ʵ�֣�Ŀǰ����������֤���񣨰��� RBAC ��Ȩ������Ʒ��������ʵ���˻�������ɾ�Ĳ鹦�ܣ��������������֤���������������֤������δ��ȫ��ɡ�

## ����ջ
### ���Ŀ��
- **.NET 9.0**����Ŀ�Ļ���������ܡ�
- **.NET Aspire**�����ڹ�����ԭ��Ӧ�ã���΢����Ĳ���͹���

### �����֤����Ȩ
- **OpenIddict**��ʵ�� OAuth 2.0 �� OpenID Connect Э�飬�ṩ�����֤����Ȩ���ܡ�
- **RBAC�����ڽ�ɫ�ķ��ʿ��ƣ�**���������֤������ʵ�֣����ڹ����û�Ȩ�ޡ�

### ���ݿ��뻺��
- **MySQL**����Ϊ��Ҫ�Ĺ�ϵ�����ݿ⣬�洢�û�����Ʒ�����ݡ�
- **Redis**�����ڻ����û�Ȩ����Ϣ�����ϵͳ���ܡ�

### ��������
- **MediatR**��ʵ�� CQRS ģʽ�е�����Ͳ�ѯ����
- **Ardalis.GuardClauses**�����ڲ�����֤����������Ľ�׳�ԡ�
- **EPPlus**�����ڴ��� Excel �ļ���

## ��Ŀ�ṹ
```plaintext
.git/
.gitattributes
.github/
.gitignore
AppHost/
  ELF.AppHost/          # Ӧ��������Ŀ�����������͹���΢����
  ELF.ServiceDefaults/  # �������������Ŀ
ERP.sln
Identity/
  .filenesting.json
  Application/          # �����֤�����Ӧ�ò�
  Domain/               # �����֤����������
  Identity.sln
  Infrastructure/       # �����֤����Ļ�����ʩ��
  Web/                  # �����֤����� Web ��
LICENSE.txt
Products/
  .filenesting.json
  Application/          # ��Ʒ��������Ӧ�ò�
  Domain/               # ��Ʒ�������������
  ELF/
  Identity.sln
  Infrastructure/       # ��Ʒ�������Ļ�����ʩ��
  Web/                  # ��Ʒ�������� Web ��
README.md
```

## ��Ҫ����ģ��
### �����֤����
- **�û���¼**��֧��������Ȩ��ˢ��������Ȩ��
- **RBAC ��Ȩ**��ͨ�� Redis �����û�Ȩ����Ϣ�������ȨЧ�ʡ�
- **Ȩ�޹���**�������û���ɫ���䲻ͬ��Ȩ�ޡ�

### ��Ʒ�������
- **���� CRUD ����**��ʵ����Ʒ��������ɾ�����޸ĺͲ�ѯ���ܡ�
- **�����֤����**�����������֤���������������֤��ȷ��ֻ����Ȩ�û����Է��ʡ�

## ����������
### ����Ҫ��
- **.NET 9.0 SDK**
- **Docker**���������� MySQL �� Redis ������

### ����
1. ��¡��Ŀ�����أ�
```bash
git clone https://github.com/your-repo/ERP.git
cd ERP
```

2. ����Ӧ��������
```bash
dotnet run --project AppHost/ELF.AppHost/ELF.AppHost.csproj
```

3. ���������֤����
```plaintext
https://localhost:8001
```

4. ������Ʒ�������
```plaintext
https://localhost:8002
```

## ע������
- ��Ʒ���������δ��ȫ��ɣ����ֹ��ܿ��ܴ���ȱʧ���ȶ��������
- ��ȷ�� Docker �������������Ա���ȷ���� MySQL �� Redis ������

## ����ָ��
�������Ϊ��Ŀ�������ף�����ѭ���²��裺
1. Fork ���ֿ⡣
2. ����һ���µķ�֧��`git checkout -b feature/your-feature`��
3. �ύ��ĸ��ģ�`git commit -m 'Add some feature'`��
4. ��������֧��`git push origin feature/your-feature`��
5. �ύ Pull Request��

## ��������
�� README �� AI ���ɣ����ܴ��ڴ�©�������ο����������ʹ�ù������������⣬�������Ŀ������ύ Issue ������