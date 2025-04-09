$(document).ready(function() {
  // Initial state
  let currentView = 'dashboard';
  
  // Load the default view on startup
  loadView('dashboard');
  
  // Handle navigation clicks
  $('.nav-pills .nav-link').on('click', function(e) {
    e.preventDefault();
    
    // Get the hash from the link
    const hash = $(this).attr('href').substring(1); // Remove the # character
    
    // Update the active class
    $('.nav-pills .nav-link').removeClass('active');
    $(this).addClass('active');
    
    // Load the corresponding view
    loadView(hash);
  });
  
  // Handle hash changes in URL (back/forward browser buttons)
  $(window).on('hashchange', function() {
    const hash = window.location.hash.substring(1) || 'dashboard';
    loadView(hash);
  });
  
  // Function to load views based on the hash
  function loadView(viewName) {
    // Default to dashboard if no hash
    if (!viewName) viewName = 'dashboard';
    
    // Check if we're loading a detail view
    if (viewName.startsWith('workout/')) {
      const id = parseInt(viewName.split('/')[1]);
      loadWorkoutDetail(id);
      return;
    }
    
    // Handle main views
    switch(viewName) {
      case 'dashboard':
        $('#app').html(renderDashboard());
        break;
      case 'workouts':
        $('#app').html(renderWorkouts());
        break;
      case 'nutrition':
        $('#app').html(renderNutrition());
        break;
      case 'profile':
        $('#app').html(renderProfile());
        break;
      default:
        $('#app').html(renderDashboard());
    }
    
    // Update the hash if needed
    if (window.location.hash !== '#' + viewName) {
      window.location.hash = viewName;
    }
    
    currentView = viewName;
  }
  
  // Render functions for each view
  function renderDashboard() {
    return `
      <div class="row">
        <div class="col-12">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1>Dashboard</h1>
            <div class="btn-group">
              <a href="#workouts" class="btn btn-outline-primary">All Workouts</a>
              <a href="#nutrition" class="btn btn-outline-primary">Nutrition</a>
            </div>
          </div>
          
          <!-- Dashboard content -->
          <!-- Cards and stats -->
        </div>
      </div>
    `;
  }
  
  function renderWorkouts() {
    // Get mock workout data
    const workouts = [
      { id: 1, date: '2025-04-05', type: 'Running', duration: 30, calories: 320, distance: 3.2 },
      { id: 2, date: '2025-04-04', type: 'Strength', duration: 45, calories: 280 },
      { id: 3, date: '2025-04-03', type: 'Yoga', duration: 60, calories: 180 }
    ];
    
    return `
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h1>Workouts</h1>
        <button class="btn btn-primary">Log New Workout</button>
      </div>
      
      <table class="table table-hover">
        <thead>
          <tr>
            <th>Date</th>
            <th>Type</th>
            <th>Duration</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          ${workouts.map(workout => `
            <tr>
              <td>${new Date(workout.date).toLocaleDateString()}</td>
              <td>${workout.type}</td>
              <td>${workout.duration} minutes</td>
              <td>
                <a href="#workout/${workout.id}" class="btn btn-sm btn-outline-primary me-1">View</a>
                <button class="btn btn-sm btn-outline-secondary me-1">Edit</button>
                <button class="btn btn-sm btn-outline-danger">Delete</button>
              </td>
            </tr>
          `).join('')}
        </tbody>
      </table>
    `;
  }
  
  function loadWorkoutDetail(id) {
    // Get mock workout data
    const workouts = [
      { id: 1, date: '2025-04-05', type: 'Running', name: 'Morning Run', duration: 30, calories: 320, distance: 3.2, intensity: 'Medium', description: 'Morning run in the park' },
      { id: 2, date: '2025-04-04', type: 'Strength', name: 'Upper Body Strength', duration: 45, calories: 280, distance: null, intensity: 'High', description: 'Upper body workout' },
      { id: 3, date: '2025-04-03', type: 'Yoga', name: 'Yoga Session', duration: 60, calories: 180, distance: null, intensity: 'Low', description: 'Relaxation yoga session' }
    ];
    
    const workout = workouts.find(w => w.id === id);
    
    if (!workout) {
      $('#app').html(`
        <div class="alert alert-danger">
          <h4>Error</h4>
          <p>Workout not found</p>
          <a href="#workouts" class="btn btn-outline-secondary">Back to Workouts</a>
        </div>
      `);
      return;
    }
    
    $('#app').html(`
      <div class="mb-4">
        <a href="#workouts" class="btn btn-outline-secondary mb-3">&laquo; Back to Workouts</a>
        <div class="d-flex justify-content-between align-items-center">
          <h1>${workout.name || workout.type}</h1>
          <button class="btn btn-primary">Edit Workout</button>
        </div>
      </div>
      
      <!-- Workout details -->
      <div class="row">
        <!-- Details go here -->
      </div>
    `);
  }
  
  function renderNutrition() {
    return `
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h1>Nutrition Tracking</h1>
        <button class="btn btn-primary">Log Meal</button>
      </div>
      
      <!-- Nutrition content -->
    `;
  }
  
  function renderProfile() {
    return `
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h1>Your Profile</h1>
        <button class="btn btn-primary">Edit Profile</button>
      </div>
      
      <!-- Profile content -->
    `;
  }
});