document.getElementById("btnLocalizar").addEventListener("click", function () {
    const select = document.getElementById("selectDepartamento");
    const id = select.value;

    if (!id) {
        alert("Selecione um departamento.");
        return;
    }

    fetch(`/Departamentos/ObterDepartamento?id=${id}`)
        .then(res => {
            if (!res.ok) throw new Error("Erro na resposta");
            return res.json();
        })
        .then(data => {
            document.getElementById("formEditar").style.display = "block";
            document.getElementById("inputId").value = data.id;
            document.getElementById("editId").value = data.id;
            document.getElementById("editNome").value = data.nome;
            document.getElementById("erro").style.display = "none";
        })
        .catch(err => {
            console.error(err);
            document.getElementById("formEditar").style.display = "none";
            document.getElementById("erro").style.display = "block";
        });
});