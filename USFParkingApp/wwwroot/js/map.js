document.addEventListener('DOMContentLoaded', function() {
    // Check if map element exists
    const mapElement = document.getElementById('map');
    if (!mapElement) return;
    
    // USF coordinates
    const usfCoordinates = [28.0587, -82.4139];
    
    // Initialize map (OpenMap API integration)
    const map = L.map('map').setView(usfCoordinates, 15);
    
    // Add OpenStreetMap tiles
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(map);
    
    // Fetch parking lot data from our API
    fetch('/api/parking-lots')
        .then(response => response.json())
        .then(parkingLots => {
            // Check for filter
            const filter = sessionStorage.getItem('parkingFilter') || 'all';
            let filteredLots = parkingLots;
            
            // Apply filter if not 'all'
            if (filter !== 'all') {
                filteredLots = parkingLots.filter(lot => 
                    lot.type.toLowerCase().includes(filter.toLowerCase())
                );
            }
            
            // Add markers for each parking lot
            filteredLots.forEach(lot => {
                if (lot.coordinates) {
                    // Determine marker color based on status
                    const colorMap = {
                        'open': '#16a34a',   // green
                        'limited': '#ca8a04', // yellow
                        'full': '#dc2626',    // red
                        'closed': '#9ca3af'   // gray
                    };
                    
                    const color = colorMap[lot.status] || '#3b82f6'; // blue default
                    
                    // Create custom icon
                    const customIcon = L.divIcon({
                        className: 'custom-div-icon',
                        html: `<div style="background-color: ${color}; width: 20px; height: 20px; border-radius: 50%; border: 2px solid white;"></div>`,
                        iconSize: [20, 20],
                        iconAnchor: [10, 10]
                    });
                    
                    // Add marker
                    const marker = L.marker([lot.coordinates.lat, lot.coordinates.lng], {
                        icon: customIcon,
                        title: lot.name
                    }).addTo(map);
                    
                    // Add popup
                    marker.bindPopup(`
                        <div>
                            <h3 style="font-weight: bold;">${lot.name}</h3>
                            <p>${lot.type}</p>
                            <p>Available: ${lot.availableSpaces} / ${lot.totalSpaces}</p>
                            <p>Status: ${lot.status.charAt(0).toUpperCase() + lot.status.slice(1)}</p>
                        </div>
                    `);
                }
            });
            
            // Check for selected lot ID
            const selectedLotId = sessionStorage.getItem('selectedLotId');
            if (selectedLotId) {
                const selectedLot = parkingLots.find(lot => lot.id === parseInt(selectedLotId));
                if (selectedLot && selectedLot.coordinates) {
                    map.setView([selectedLot.coordinates.lat, selectedLot.coordinates.lng], 17);
                    
                    // Clear selection
                    sessionStorage.removeItem('selectedLotId');
                }
            }
        })
        .catch(error => {
            console.error('Error loading parking lot data:', error);
            mapElement.innerHTML = '<div class="alert alert-danger m-3">Error loading map data. Please try again later.</div>';
        });
    
    // Add legend to the map
    const legend = L.control({ position: 'bottomright' });
    legend.onAdd = function() {
        const div = L.DomUtil.create('div', 'map-legend');
        div.innerHTML = `
            <div style="background: white; padding: 10px; border-radius: 5px; box-shadow: 0 1px 5px rgba(0,0,0,0.4);">
                <h4 style="margin: 0 0 5px 0; font-size: 14px; font-weight: bold;">Map Legend</h4>
                <div style="display: flex; align-items: center; margin: 3px 0;">
                    <span style="display: inline-block; width: 12px; height: 12px; border-radius: 50%; background-color: #16a34a; margin-right: 5px;"></span>
                    <span style="font-size: 12px;">Available (>25%)</span>
                </div>
                <div style="display: flex; align-items: center; margin: 3px 0;">
                    <span style="display: inline-block; width: 12px; height: 12px; border-radius: 50%; background-color: #ca8a04; margin-right: 5px;"></span>
                    <span style="font-size: 12px;">Limited (5-25%)</span>
                </div>
                <div style="display: flex; align-items: center; margin: 3px 0;">
                    <span style="display: inline-block; width: 12px; height: 12px; border-radius: 50%; background-color: #dc2626; margin-right: 5px;"></span>
                    <span style="font-size: 12px;">Full (<5%)</span>
                </div>
                <div style="display: flex; align-items: center; margin: 3px 0;">
                    <span style="display: inline-block; width: 12px; height: 12px; border-radius: 50%; background-color: #9ca3af; margin-right: 5px;"></span>
                    <span style="font-size: 12px;">Closed</span>
                </div>
            </div>
        `;
        return div;
    };
    legend.addTo(map);
});