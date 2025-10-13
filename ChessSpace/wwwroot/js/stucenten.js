//document.addEventListener('DOMContentLoaded', () => {
//    const transactions = [
//        { desc: 'Boodschappen - AH', amt: -18.40, date: 'Vandaag' },
//        { desc: 'Studievergoeding', amt: 150.00, date: '2 dgn' },
//    ];
//    const incomes = [
//        { desc: 'Bijbaan', amt: 80.00, date: 'Vandaag' },
//        { desc: 'Stufi', amt: 300.00, date: '4 dgn' },
//    ];
//    const expenses = [
//        { desc: 'Netflix', amt: -7.99, date: '2 dgn' },
//        { desc: 'Lunch', amt: -6.20, date: '5 dgn' },
//    ];

//    const txList = document.getElementById('txList');
//    const incomeList = document.getElementById('incomeList');
//    const expenseList = document.getElementById('expenseList');
//    const incomeTotal = document.getElementById('incomeTotal');
//    const expenseTotal = document.getElementById('expenseTotal');
//    const budgetDiff = document.getElementById('budgetDiff');
//    const savingsLeft = document.getElementById('savingsLeft');
//    const customBudgetInput = document.getElementById('customBudget');
//    const budgetProgress = document.getElementById('budgetProgress');
//    const budgetPercent = document.getElementById('budgetPercent');

//    let savedBudget = 0;

//    function formatMoney(n) {
//        return (n < 0 ? '- ' : '+ ') + '€ ' + Math.abs(n).toFixed(2);
//    }

//    function calcTotal(data) {
//        return data.reduce((sum, t) => sum + t.amt, 0);
//    }

//    function render(list, data) {
//        list.innerHTML = '';
//        data.forEach((t, i) => {
//            const el = document.createElement('div');
//            el.className = 'tx';
//            el.innerHTML = `
//                <div>
//                    <div class="desc" contenteditable="true">${t.desc}</div>
//                    <div class='meta'>${t.date}</div>
//                </div>
//                <div style='text-align:right'>
//                    <div class='amount' contenteditable="true" style='font-weight:700;color:${t.amt < 0 ? "#ef4444" : "#059669"}'>
//                        ${formatMoney(t.amt)}
//                    </div>
//                    <div class='meta'>${t.amt < 0 ? "Uitgave" : "Inkomen"}</div>
//                </div>`;
//            list.appendChild(el);

//            el.querySelector('.desc').addEventListener('blur', e => {
//                data[i].desc = e.target.textContent.trim();
//            });
//            el.querySelector('.amount').addEventListener('blur', e => {
//                const v = e.target.textContent.replace(/[^\d-.,]/g, '').replace(',', '.');
//                data[i].amt = parseFloat(v) || 0;
//                updateAll();
//            });
//        });
//    }

//    function updateTotals() {
//        const totalIncomes = calcTotal(incomes);
//        const totalExpenses = calcTotal(expenses);
//        incomeTotal.textContent = 'Totaal: € ' + totalIncomes.toFixed(2);
//        expenseTotal.textContent = 'Totaal: € ' + Math.abs(totalExpenses).toFixed(2);

//        const diff = totalIncomes + totalExpenses;
//        budgetDiff.textContent = 'Beschikbaar budget: € ' + diff.toFixed(2);

//        const leftover = diff - savedBudget;
//        savingsLeft.textContent = 'Over als spaargeld: € ' + leftover.toFixed(2);

//        const percent = Math.min(100, Math.max(0, (savedBudget > 0 ? (diff / savedBudget * 100) : 0)));
//        budgetProgress.style.width = percent + '%';
//        budgetPercent.textContent = percent.toFixed(1) + '% van budget benut';
//    }

//    function updateAll() {
//        render(txList, transactions);
//        render(incomeList, incomes);
//        render(expenseList, expenses);
//        updateTotals();
//    }

//    updateAll();

//    document.getElementById('addTx').addEventListener('click', () => {
//        transactions.unshift({ desc: 'Nieuwe transactie', amt: 0, date: 'Nu' });
//        updateAll();
//    });
//    document.getElementById('addIncome').addEventListener('click', () => {
//        incomes.unshift({ desc: 'Nieuwe inkomsten', amt: 0, date: 'Nu' });
//        updateAll();
//    });
//    document.getElementById('addExpense').addEventListener('click', () => {
//        expenses.unshift({ desc: 'Nieuwe uitgave', amt: -0, date: 'Nu' });
//        updateAll();
//    });

//    document.getElementById('saveBudget').addEventListener('click', () => {
//        savedBudget = parseFloat(customBudgetInput.value) || 0;
//        updateTotals();
//    });

//    document.getElementById('saveNow').addEventListener('click', () => {
//        const p = document.getElementById('progressBar');
//        const cur = parseFloat(p.style.width);
//        p.style.width = Math.min(96, cur + 6) + '%';
//        document.getElementById('goalMeta').textContent =
//            Math.round(parseFloat(p.style.width) / 100 * 517) + ' / 517 gespaard';
//    });

//    const coin = document.querySelector('.coin');
//    window.addEventListener('mousemove', e => {
//        const x = (e.clientX / window.innerWidth - 0.5) * 18;
//        const y = (e.clientY / window.innerHeight - 0.5) * 12;
//        coin.style.transform = `translate(${x}px, ${y}px) rotate(-12deg)`;
//    });
//});
