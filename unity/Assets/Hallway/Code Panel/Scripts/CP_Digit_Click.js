//��� ��������� �������� � ���������� ��� ������� �� ��������, � ������� ���������� ������
var child_to_glow = "";
var digit = 0;

//���� �������� ���������� ����� ���� � �������� ���� ��� ����� �����
//(���� � ���������� ��, �������, ���� ����������, �� ������ �������� ��� ������� �� �����)
var code_length = 5;

function OnMouseDown() {
	//�� ��������� "����������" ������� ��������� ������ ������� Update,
	//������� ���� �������� ����� �������
	if (this.enabled) {
		//���� ������������ �� ��������, ������� ������� ���������
		//������ ����� �� ����� �� ����� ������ (B<�����> Group)
		//�������� - ��� ����� ������ ������� ������ Code Panel
		//����� ����� �������� ���� ������ Field Group, ����� �� ����� - ����� Field Text
	
		//����� ����� �� ����� ����� ���� �������� ����� GameObject.Find(<���>) ���� �� ������,
		//�� ��� �� �� ������ ������ ������������� � ����������� � ����� �����,
		//����� � �����-�� �� ���� ���� �������� ���� ������ � ������ Field Text?
	
		var txt = transform.parent.transform.Find("Field Group").transform.Find("Field Text");
		if (txt.GetComponent(TextMesh).text.length >= code_length) {
			transform.Find(child_to_glow).animation.Play("CP ButtonError");
		} else {
			transform.Find(child_to_glow).animation.Play("CP ButtonLight");
			txt.GetComponent(TextMesh).text += digit;
		}
	}
}