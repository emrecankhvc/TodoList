// wwwroot/js/todo-calendar.js
class TodoCalendar {
    constructor(options = {}) {
        this.currentYear = new Date().getFullYear();
        this.currentMonth = new Date().getMonth() + 1;
        this.calendarData = null;
        this.apiBaseUrl = options.apiBaseUrl || '/Todo';
        this.localizations = options.localizations || {};
        this.init();
    }

    init() {
        this.bindEvents();
        this.loadCalendarData();
    }

    bindEvents() {
        const prevBtn = document.getElementById('prev-month');
        const nextBtn = document.getElementById('next-month');

        if (prevBtn) {
            prevBtn.addEventListener('click', () => this.navigateMonth(-1));
        }

        if (nextBtn) {
            nextBtn.addEventListener('click', () => this.navigateMonth(1));
        }
    }

    navigateMonth(direction) {
        this.currentMonth += direction;

        if (this.currentMonth < 1) {
            this.currentMonth = 12;
            this.currentYear--;
        } else if (this.currentMonth > 12) {
            this.currentMonth = 1;
            this.currentYear++;
        }

        this.loadCalendarData();
    }

    async loadCalendarData() {
        try {
            const url = `${this.apiBaseUrl}/GetCalendarData?year=${this.currentYear}&month=${this.currentMonth}`;
            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            this.calendarData = await response.json();
            this.renderCalendar();
        } catch (error) {
            console.error('Calendar data loading error:', error);
            this.showError('Calendar data could not be loaded.');
        }
    }

    renderCalendar() {
        if (!this.calendarData) return;

        this.updateCalendarTitle();
        this.renderCalendarDays();
    }

    updateCalendarTitle() {
        const titleElement = document.getElementById('calendar-title');
        if (titleElement) {
            titleElement.textContent = this.calendarData.monthName;
        }
    }

    renderCalendarDays() {
        const grid = document.getElementById('calendar-grid');
        if (!grid) return;

        // Preserve weekday headers
        const weekdayHeaders = Array.from(grid.querySelectorAll('.calendar-weekday'));
        grid.innerHTML = '';
        weekdayHeaders.forEach(header => grid.appendChild(header));

        // Add empty cells for first week alignment
        this.addEmptyDays(grid);

        // Add calendar days
        this.calendarData.calendarDays.forEach(day => {
            const dayElement = this.createDayElement(day);
            grid.appendChild(dayElement);
        });
    }

    addEmptyDays(grid) {
        const firstDayOfWeek = this.calendarData.firstDayOfWeek;
        const adjustedFirstDay = firstDayOfWeek === 0 ? 6 : firstDayOfWeek - 1;

        for (let i = 0; i < adjustedFirstDay; i++) {
            const emptyDay = document.createElement('div');
            emptyDay.className = 'calendar-day other-month';
            grid.appendChild(emptyDay);
        }
    }

    createDayElement(dayData) {
        const dayElement = document.createElement('div');
        dayElement.className = 'calendar-day';
        dayElement.textContent = dayData.day;

        this.applyDayStyles(dayElement, dayData);
        this.attachDayEvents(dayElement, dayData);

        return dayElement;
    }

    applyDayStyles(element, dayData) {
        // Today styling
        if (this.isToday(dayData.date)) {
            element.classList.add('today');
        }

        // Todo-related styling
        if (dayData.hasTodos) {
            element.classList.add('has-todos');
            this.applyPriorityStyle(element, dayData.priorityColor);
            this.applyStatusStyle(element, dayData.statusColor);
            this.addTooltip(element, dayData);
        }
    }

    isToday(dateString) {
        const today = new Date().toISOString().split('T')[0];
        return dateString.includes(today);
    }

    applyPriorityStyle(element, priorityColor) {
        const priorityMap = {
            '#dc3545': 'priority-high',
            '#ffc107': 'priority-medium',
            '#28a745': 'priority-low'
        };

        const priorityClass = priorityMap[priorityColor] || 'no-priority';
        element.classList.add(priorityClass);
    }

    applyStatusStyle(element, statusColor) {
        const statusMap = {
            '#28a745': 'status-completed',
            '#ffc107': 'status-partial',
            '#dc3545': 'status-pending'
        };

        const statusClass = statusMap[statusColor];
        if (statusClass) {
            element.classList.add(statusClass);
        }
    }

    addTooltip(element, dayData) {
        const todoTitles = dayData.todos.map(todo => todo.title).join(', ');
        element.title = `${dayData.todoCount} ${this.localizations.tasks || 'tasks'}: ${todoTitles}`;
    }

    attachDayEvents(element, dayData) {
        if (dayData.hasTodos) {
            element.addEventListener('click', () => this.showDayTodos(dayData));
            element.style.cursor = 'pointer';
        }
    }

    showDayTodos(dayData) {
        // This could be enhanced with a modal instead of alert
        const todoList = dayData.todos.map(todo =>
            `• ${todo.title} (${this.getPriorityText(todo.priority)}) - ${this.getStatusText(todo.status)}`
        ).join('\n');

        alert(`${dayData.day} ${this.calendarData.monthName} - ${this.localizations.tasks || 'Tasks'}:\n\n${todoList}`);
    }

    getPriorityText(priority) {
        const priorities = this.localizations.priorities || {
            'High': 'High',
            'Medium': 'Medium',
            'Low': 'Low'
        };
        return priorities[priority] || priority;
    }

    getStatusText(status) {
        const statuses = this.localizations.statuses || {
            'C': 'Completed',
            'I': 'In Progress',
            'U': 'Uncompleted'
        };
        return statuses[status] || status;
    }

    showError(message) {
        // You could integrate with your toast notification system here
        console.error(message);
    }
}

// Export for module usage
if (typeof module !== 'undefined' && module.exports) {
    module.exports = TodoCalendar;
}